using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FunctionalExtentions
{
    public static class ReflectionTypeExtentions
    {
        public static bool IsGenericOfType(this Type type, Type genericTypeDefinition)
        {
            return type.FindGenericTypeDefinition(genericTypeDefinition) != null;
        }

        public static Type[] GetGenericArguments(this Type type, Type genericTypeDefinition)
        {
            var locatedTypeDefinition = type.FindGenericTypeDefinition(genericTypeDefinition);
            return locatedTypeDefinition == null
                ? Type.EmptyTypes // TODO do we want this behavior?
                : locatedTypeDefinition.GetGenericArguments();
        }

        private static Type FindGenericTypeDefinition(this Type type, Type genericTypeDefinition)
        {
            ThrowHelper.ThrowIfNull(type, nameof(type));
            ThrowHelper.ThrowIfNull(genericTypeDefinition, nameof(genericTypeDefinition));
            ThrowHelper.ThrowIf(!genericTypeDefinition.IsGenericTypeDefinition, nameof(genericTypeDefinition),
                "must be a generic type definition (e. g. typeof(List<>))");

            if (genericTypeDefinition.IsInterface)
            {
                return type.GetInterfaces().FirstOrDefault(
                    i => i == genericTypeDefinition
                        || (i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition)
                );
            }

            for (var currentType = type; currentType != null; currentType = currentType.BaseType)
            {
                if (currentType == genericTypeDefinition
                    || (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == genericTypeDefinition))
                {
                    return currentType;
                }
            }

            return null;
        }

        public static bool CanBeNull(this Type type)
        {
            ThrowHelper.ThrowIfNull(type, nameof(type));

            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        public static object GetDefaultValue(this Type type)
        {
            ThrowHelper.ThrowIfNull(type, nameof(type));

            return type.IsValueType ? DynamicActivator.MakeObject(type) : null;
        }

        public static MethodInfo GetMethod(Expression<Action> methodCallExpression)
        {
            LambdaExpression lambda = methodCallExpression;
            return GetMethod(lambda);
        }

        public static MethodInfo GetMethod<TInstance>(Expression<Action<TInstance>> methodCallExpression)
        {
            LambdaExpression lambda = methodCallExpression;
            return GetMethod(lambda);
        }

        private static MethodInfo GetMethod(LambdaExpression methodCallExpression)
        {
            ThrowHelper.ThrowIfNull(methodCallExpression, nameof(methodCallExpression));

            var methodCall = methodCallExpression.Body as MethodCallExpression;

            ThrowHelper.ThrowIf(methodCall == null,
                nameof(methodCall),
                "methodCallExpression: the body of the lambda expression must be a method call. Found: "
                + methodCallExpression.Body.NodeType);

            return methodCall.Method;
        }

        public static PropertyInfo GetProperty<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            LambdaExpression lambda = propertyExpression;
            return GetProperty(lambda);
        }

        public static PropertyInfo GetProperty<TInstance, TProperty>(Expression<Func<TInstance, TProperty>> propertyExpression)
        {
            LambdaExpression lambda = propertyExpression;
            return GetProperty(lambda);
        }

        private static PropertyInfo GetProperty(LambdaExpression propertyExpression)
        {
            ThrowHelper.ThrowIfNull(propertyExpression, "propertyExpression");
            var property = propertyExpression.Body as MemberExpression;
            if (property == null || property.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException("propertyExpression: the body of the lambda expression must be a property access. Found: " + propertyExpression.Body.NodeType);
            }

            return (PropertyInfo)property.Member;
        }

        public static Type GetDelegateType(this MethodInfo method)
        {
            ThrowHelper.ThrowIfNull(method, nameof(method));

            var parameters = method.GetParameters();
            if (parameters.Length < 16
                && !parameters.Any(p => p.ParameterType.IsByRef))
            {
                var isAction = method.ReturnType == typeof(void);
                var typeName = isAction
                    ? parameters.Length == 0
                        ? "System.Action"
                        : "System.Action`" + parameters.Length
                    : "System.Func`" + (parameters.Length + 1);
                var genericTypeDefinition = typeof(Action).Assembly.GetType(typeName, throwOnError: true);
                if (!genericTypeDefinition.IsGenericTypeDefinition)
                {
                    return genericTypeDefinition; // only true for Action
                }

                var genericTypeArguments = new Type[parameters.Length + (isAction ? 1 : 0)];
                for (var i = 0; i < parameters.Length; ++i)
                {
                    genericTypeArguments[i] = parameters[i].ParameterType;
                }
                if (!isAction)
                {
                    genericTypeArguments[genericTypeArguments.Length - 1] = method.ReturnType;
                }
                return genericTypeDefinition.MakeGenericType(genericTypeArguments);
            }

            var parameterExpressions = new ParameterExpression[parameters.Length];
            for (var i = 0; i < parameters.Length; ++i)
            {
                parameterExpressions[i] = Expression.Parameter(parameters[i].ParameterType);
            }

            var lambda = Expression.Lambda(
                Expression.Call(
                    method.IsStatic ? null : Expression.Parameter(method.DeclaringType),
                    method,
                    parameterExpressions
                ),
                parameterExpressions
            );
            return lambda.Type;
        }

        public static bool IsCastableTo(this Type from, Type to)
        {
            ThrowHelper.ThrowIfNull(from, nameof(from));
            ThrowHelper.ThrowIfNull(to, nameof(to));

            return CastHelper.CanExplicitCast(from, to);
        }

        public static bool IsImplicitlyCastableTo(this Type from, Type to)
        {
            ThrowHelper.ThrowIfNull(from, nameof(from));
            ThrowHelper.ThrowIfNull(to, nameof(to));

            return CastHelper.CanImplicitCast(from, to);
        }
    }
}