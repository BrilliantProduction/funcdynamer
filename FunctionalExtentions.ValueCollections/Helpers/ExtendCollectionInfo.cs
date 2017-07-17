namespace FunctionalExtentions.ValueCollections
{
    internal struct ExtendCollectionInfo
    {
        public int Capacity { get; }

        public int Count { get; }

        public int DefaultCapacity { get; }

        public int DefaultGrowingRate { get; }

        public double GrowingScaleLimit { get; }

        public double GrowingScale { get; }

        public ExtendCollectionInfo(int capacity, int count, int defCapacity,
            int defGrowRate, double growScaleLimit, double growScale)
        {
            Capacity = capacity;
            Count = count;
            DefaultCapacity = defCapacity;
            DefaultGrowingRate = defGrowRate;
            GrowingScale = growScale;
            GrowingScaleLimit = growScaleLimit;
        }
    }
}