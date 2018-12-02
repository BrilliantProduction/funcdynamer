namespace FunctionalExtentions.Abstract.Collections
{
    public interface IStack<T> : IValueCollection<T>
    {
        T Peek();

        T Pop();

        void Push(T item);

        T[] ToArray();

        void TrimExcess();
    }
}