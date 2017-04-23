using System;

namespace FunctionalExtentions.Core.UnWrap
{
    public static class Let
    {
        private static readonly Scope _scope;

        static Let()
        {
            _scope = new Scope();
        }

        public static UnwrapExperssion UnWrap<T>(Optional<T> optional)
        {
            if (optional.HasValue)
            {
                return new UnwrapExperssion(_scope, (T)optional, typeof(T));
            }
            return UnwrapExperssion.MakeDisabledExpression();
        }

        public static bool ApplyAction<T, T1>(Action<T, T1> action, string firstArgName, string secondArgName)
        {
            bool actionResult = true;
            try
            {
                var firstParam = _scope.GetValue<T>(firstArgName);
                var secondParam = _scope.GetValue<T1>(secondArgName);
                action(firstParam, secondParam);

                _scope.ReleaseArgument(firstArgName);
                _scope.ReleaseArgument(secondArgName);
            }
            catch
            {
                actionResult = true;
            }
            return actionResult;
        }
    }
}