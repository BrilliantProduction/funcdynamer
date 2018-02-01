namespace FunctionalExtentions.Abstract.ValueCollections
{
    public interface IQueue<T> : IValueCollection<T>
    {
        void Enqueue(T item);

        T Dequeue();

        T Peek();
    }
}
