using FunctionalExtentions.Core;
using System;

namespace FunctionalExtentions.Core.Currying
{
    public class CurryHelper
    {
        public static Func<A, Func<B, Result>> Curry<A, B, Result>(Func<A, B, Result> function)
        {
            return (A a) => (B b) => function(a, b);
        }

        public static Func<A, Func<B, Func<C, Result>>> Curry<A, B, C, Result>(Func<A, B, C, Result> function)
        {
            return (A a) => (B b) => (C c) => function(a, b, c);
        }

        public static Func<A, Func<B, Func<C, Func<D, Result>>>> Curry<A, B, C, D, Result>(Func<A, B, C, D, Result> function)
        {
            return (A a) => (B b) => (C c) => (D d) => function(a, b, c, d);
        }

        public static Func<A, Func<B, Func<C, Func<D, Func<E, Result>>>>> Curry<A, B, C, D, E, Result>(Func<A, B, C, D, E, Result> function)
        {
            return (A a) => (B b) => (C c) => (D d) => (E e) => function(a, b, c, d, e);
        }

        public static Func<A, Func<B, Func<C, Func<D, Func<E, Func<F, Result>>>>>> Curry<A, B, C, D, E, F, Result>(Func<A, B, C, D, E, F, Result> function)
        {
            return (A a) => (B b) => (C c) => (D d) => (E e) => (F f) => function(a, b, c, d, e, f);
        }

        public static Optional<R> Apply<A, R>(Func<A, Optional<R>> creator, Optional<A> param)
        {
            if (creator != null)
            {
                if (param.HasValue)
                {
                    return creator(param.Value);
                }
            }
            return Optional<R>.CreateOptional();
        }
    }
}