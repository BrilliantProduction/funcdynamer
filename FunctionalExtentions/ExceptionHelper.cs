using FunctionalExtentions.Abstract;
using System;

namespace FunctionalExtentions
{
    public static class ExceptionHelper
    {
        public static void HandleUnsafeAction<TException, TResultException>
            (Action action, IExceptionConverter<TException, TResultException> converter)
            where TException : Exception
            where TResultException : Exception
        {
            try
            {
                action();
            }
            catch (TException exception)
            {
                throw converter.ConvertFrom(exception);
            }
        }

        public static TResult HandleUnsafeOperation<TException, TResultException, TResult>
            (Func<TResult> operation, IExceptionConverter<TException, TResultException> converter)
            where TException : Exception
            where TResultException : Exception
        {
            try
            {
                return operation();
            }
            catch (TException exception)
            {
                throw converter.ConvertFrom(exception);
            }
        }
    }
}