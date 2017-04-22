using System;
using FunctionalExtentions.Abstract;

namespace FunctionalExtentions
{
    public class ExceptionConverter<TException, TResultException> : IExceptionConverter<TException, TResultException>
        where TException : Exception
        where TResultException : Exception
    {
        private readonly Func<TException, TResultException> _converter;

        public ExceptionConverter(Func<TException, TResultException> converter)
        {
            _converter = converter;
        }

        public TResultException ConvertFrom(TException exception) => _converter(exception);
    }
}