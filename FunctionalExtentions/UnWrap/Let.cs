using System;

namespace FunctionalExtentions.Core.Unwrap
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
                actionResult = false;
            }
            return actionResult;
        }

        public static bool ApplyAction<T, T1, T2>(Action<T, T1, T2> action, string[] argNames)
        {
            bool actionResult = true;

            if (argNames == null || argNames.Length != 3)
            {
                return false;
            }

            try
            {
                var firstParam = _scope.GetValue<T>(argNames[0]);
                var secondParam = _scope.GetValue<T1>(argNames[1]);
                var thirdParam = _scope.GetValue<T2>(argNames[2]);
                action(firstParam, secondParam, thirdParam);

                _scope.ReleaseArgument(argNames[0]);
                _scope.ReleaseArgument(argNames[1]);
                _scope.ReleaseArgument(argNames[2]);
            }
            catch
            {
                actionResult = false;
            }
            return actionResult;
        }

        public static bool ApplyAction<T, T1, T2, T3>(Action<T, T1, T2, T3> action, string[] argNames)
        {
            bool actionResult = true;

            if (argNames == null || argNames.Length != 4)
            {
                return false;
            }

            try
            {
                var firstParam = _scope.GetValue<T>(argNames[0]);
                var secondParam = _scope.GetValue<T1>(argNames[1]);
                var thirdParam = _scope.GetValue<T2>(argNames[2]);
                var forthParam = _scope.GetValue<T3>(argNames[3]);
                action(firstParam, secondParam, thirdParam, forthParam);

                _scope.ReleaseArgument(argNames[0]);
                _scope.ReleaseArgument(argNames[1]);
                _scope.ReleaseArgument(argNames[2]);
                _scope.ReleaseArgument(argNames[3]);
            }
            catch
            {
                actionResult = false;
            }
            return actionResult;
        }

        public static bool ApplyAction<T, T1, T2, T3, T4>(Action<T, T1, T2, T3, T4> action, string[] argNames)
        {
            bool actionResult = true;

            if (argNames == null || argNames.Length != 5)
            {
                return false;
            }

            try
            {
                var firstParam = _scope.GetValue<T>(argNames[0]);
                var secondParam = _scope.GetValue<T1>(argNames[1]);
                var thirdParam = _scope.GetValue<T2>(argNames[2]);
                var forthParam = _scope.GetValue<T3>(argNames[3]);
                var fifthParam = _scope.GetValue<T4>(argNames[4]);
                action(firstParam, secondParam, thirdParam, forthParam, fifthParam);

                _scope.ReleaseArgument(argNames[0]);
                _scope.ReleaseArgument(argNames[1]);
                _scope.ReleaseArgument(argNames[2]);
                _scope.ReleaseArgument(argNames[3]);
                _scope.ReleaseArgument(argNames[4]);
            }
            catch
            {
                actionResult = false;
            }
            return actionResult;
        }

        public static bool ApplyAction<T, T1, T2, T3, T4, T5>(Action<T, T1, T2, T3, T4, T5> action, string[] argNames)
        {
            bool actionResult = true;

            if (argNames == null || argNames.Length != 6)
            {
                return false;
            }

            try
            {
                var firstParam = _scope.GetValue<T>(argNames[0]);
                var secondParam = _scope.GetValue<T1>(argNames[1]);
                var thirdParam = _scope.GetValue<T2>(argNames[2]);
                var forthParam = _scope.GetValue<T3>(argNames[3]);
                var fifthParam = _scope.GetValue<T4>(argNames[4]);
                var sixthParam = _scope.GetValue<T5>(argNames[5]);
                action(firstParam, secondParam, thirdParam, forthParam, fifthParam, sixthParam);

                _scope.ReleaseArgument(argNames[0]);
                _scope.ReleaseArgument(argNames[1]);
                _scope.ReleaseArgument(argNames[2]);
                _scope.ReleaseArgument(argNames[3]);
                _scope.ReleaseArgument(argNames[4]);
                _scope.ReleaseArgument(argNames[5]);
            }
            catch
            {
                actionResult = false;
            }
            return actionResult;
        }
    }
}