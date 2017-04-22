using System;

namespace FunctionalExtentions.Abstract
{
    public interface IExceptionConverter<TException, TResultException>
        where TException : Exception
        where TResultException : Exception
    {
        TResultException ConvertFrom(TException exception);
    }
}