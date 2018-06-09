namespace FunctionalExtentions.Collections
{
    public struct ExtendCollectionInfo
    {
        public int Capacity { get; }

        public int Count { get; }

        public int DefaultCapacity { get; }

        public int DefaultGrowingRate { get; }

        public double GrowingScaleLimit { get; }

        public double GrowingScale { get; }

        public ExtendCollectionInfo(int count, int capacity, int defaultCapacity,
            int defaultGrowRate, double growingScaleLimit, double growingScale)
        {
            Capacity = capacity;
            Count = count;
            DefaultCapacity = defaultCapacity;
            DefaultGrowingRate = defaultGrowRate;
            GrowingScale = growingScale;
            GrowingScaleLimit = growingScaleLimit;
        }
    }
}