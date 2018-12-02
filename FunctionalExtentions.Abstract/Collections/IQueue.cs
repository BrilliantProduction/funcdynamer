namespace FunctionalExtentions.Abstract.Collections
{
    public interface IQueue<T> : IValueCollection<T>
    {
        void Enqueue(T item);

        T Dequeue();

        T Peek();
    }
}
