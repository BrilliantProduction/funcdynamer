using FunctionalExtentions.Abstract.ExceptionConverter;
using System;

namespace FunctionalExtentions.Core.UnsafeHandler
{
    public static class ExceptionHelper
    {
        public static void HandleUnsafeAction<TException, TResultException>
            (Action action, ITransformer<TException, TResultException> converter)
            where TException : Exception
            where TResultException : Exception
        {
            try
            {
                action();
            }
            catch (TException exception)
            {
                throw converter.Transform(exception);
            }
        }

        public static TResult HandleUnsafeOperation<TException, TResultException, TResult>
            (Func<TResult> operation, ITransformer<TException, TResultException> converter)
            where TException : Exception
            where TResultException : Exception
        {
            try
            {
                return operation();
            }
            catch (TException exception)
            {
                throw converter.Transform(exception);
            }
        }
    }
}