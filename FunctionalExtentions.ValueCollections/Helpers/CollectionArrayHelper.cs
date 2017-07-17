using System;

namespace FunctionalExtentions.ValueCollections
{
    internal static class CollectionArrayHelper
    {
        public static bool CheckCapacity(int additionalPart, ExtendCollectionInfo collectionInfo)
        {
            var newCount = collectionInfo.Count + additionalPart;
            var capacityGrowRate = collectionInfo.Capacity * collectionInfo.GrowingScaleLimit;
            return capacityGrowRate < newCount;
        }

        public static int GetScalingPoint(int newElementsCount, ExtendCollectionInfo collectionInfo)
        {
            int result;

            if (collectionInfo.Capacity == 0)
            {
                if (collectionInfo.DefaultGrowingRate > newElementsCount)
                {
                    result = collectionInfo.DefaultCapacity;
                }
                else
                {
                    result = (int)Math.Round(collectionInfo.Capacity * collectionInfo.GrowingScale);
                }
            }
            else
            {
                var tempResult = 0;
                do
                {
                    result = tempResult + (int)Math.Round(collectionInfo.Capacity * collectionInfo.GrowingScale);
                    tempResult = result;
                }
                while (collectionInfo.Capacity + result < collectionInfo.Capacity + newElementsCount);
            }

            return result;
        }
    }
}