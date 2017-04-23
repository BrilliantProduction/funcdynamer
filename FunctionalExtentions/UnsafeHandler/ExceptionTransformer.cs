using System;
using FunctionalExtentions.Abstract.ExceptionConverter;

namespace FunctionalExtentions.Core.UnsafeHandler
{
    public class ExceptionTransformer<TException, TResultException> : ITransformer<TException, TResultException>
        where TException : Exception
        where TResultException : Exception
    {
        private readonly Func<TException, TResultException> _converter;

        private ExceptionTransformer(Func<TException, TResultException> converter)
        {
            _converter = converter;
        }

        public static ExceptionTransformer<TException, TResultException> MakeTransformer(Func<TException, TResultException> converter)
        {
            if (converter == null)
            {
                throw new InvalidTransformerException();
            }
            return new ExceptionTransformer<TException, TResultException>(converter);
        }

        public TResultException Transform(TException exception) => _converter(exception);
    }
}