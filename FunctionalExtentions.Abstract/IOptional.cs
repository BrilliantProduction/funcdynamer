namespace FunctionalExtentions.Abstract
{
    public interface IOptional<Wrapped>
    {
        bool HasValue { get; }

        Wrapped Value { get; }

        Wrapped GetValueOrDefault();

        Wrapped GetValueOrDefault(Wrapped defaultValue);

        T Cast<T>();
    }
}