namespace FunctionalExtentions
{
    public class TypeConverterChecker<TFrom, TTo>
    {
        public bool CanConvert { get; private set; }

        public TypeConverterChecker()
        {
            TFrom from;
            if (typeof(TFrom).IsValueType)
                from = default(TFrom);
            else
                from = DynamicActivator.MakeObject<TFrom>();
            try
            {
                TTo to = (TTo)(dynamic)from;
                CanConvert = true;
            }
            catch
            {
                CanConvert = false;
            }
        }
    }
}