using FunctionalExtentions.Collections;
using NUnit.Framework;
using System.Collections.Generic;

namespace FunctionalExtentions.Tests
{
    [TestFixture]
    public class OptionalCollectionTest
    {
        [Test]
        public void CreateOptionalCollectionFromList()
        {
            //Arrange
            var list = new List<int>() { 1, 1, 1, 1, 1, 1, 1 };

            //Act
            var optionalList = list.AsOptional();

            //Assert
            Assert.IsNotNull(optionalList);
        }

        [Test]
        public void AddToOptionalCollectionWrapsToOptional()
        {
            //Arrange
            var collection = new List<int>() { 1, 2, 3, 4 };
            var optional = collection.AsOptional();

            //Act
            optional.Add(22);

            //Assert
            Assert.AreEqual(5, optional.Count);
        }
    }
}