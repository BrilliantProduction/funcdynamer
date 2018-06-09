using System;

namespace FunctionalExtentions.Collections
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

            if (collectionInfo.Capacity == 0 && collectionInfo.DefaultGrowingRate > newElementsCount)
            {
                result = collectionInfo.DefaultCapacity;
            }
            else
            {
                var tempResult = 0;

                var multiplier = collectionInfo.Capacity > 0 ?
                    collectionInfo.Capacity : collectionInfo.DefaultCapacity;

                do
                {
                    result = tempResult + (int)Math.Round(multiplier * collectionInfo.GrowingScale);
                    tempResult = result;
                }
                while ((collectionInfo.Capacity + result) * collectionInfo.GrowingScaleLimit
                < collectionInfo.Capacity + newElementsCount);
            }

            return result;
        }
    }
}