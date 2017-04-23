using System;

namespace FunctionalExtentions.Abstract.ExceptionConverter
{
    public interface ITransformer<TException, TResultException>
        where TException : Exception
        where TResultException : Exception
    {
        TResultException Transform(TException exception);
    }
}