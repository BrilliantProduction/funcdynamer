namespace FunctionalExtentions
{
    public class ImplicitTypeConversionChecker<TSource, TDestination>
    {
        public bool CanConvert { get; private set; }

        public ImplicitTypeConversionChecker()
        {
            TSource from;
            if (typeof(TSource).IsValueType)
                from = default(TSource);
            else
                from = DynamicActivator.MakeObject<TSource>();
            try
            {
                TDestination to = (dynamic)from;
                CanConvert = true;
            }
            catch
            {
                CanConvert = false;
            }
        }
    }
}