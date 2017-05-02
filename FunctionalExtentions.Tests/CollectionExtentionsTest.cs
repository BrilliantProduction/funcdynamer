using FunctionalExtentions.Abstract;
using FunctionalExtentions.Collections;
using FunctionalExtentions.Core;
using NUnit.Framework;

namespace FunctionalExtentions.Tests
{
    [TestFixture]
    public class CollectionExtentionsTest
    {
        [Test]
        public void CollectionIntFilterTest()
        {
            //Arrange
            var collection = new int[] { 1, 11, 1, 1, 1, 1, 1, 1, 1, 20, 50, 80, 100 };

            //Act
            var filtered = collection.Filter(x => x > 10);

            //Assert
            Assert.IsNotEmpty(filtered);
            Assert.AreEqual(5, filtered.Count);
        }

        [Test]
        public void CollectionStringMapTest()
        {
            //Arrange
            var collection = new string[] { "1", "2", "3", "4" };
            var expected = new int[] { 1, 2, 3, 4 };

            //Act
            var mapped = collection.Map(x => int.Parse(x));

            //Assert
            Assert.IsNotEmpty(mapped);
            Assert.AreEqual(4, mapped.Count);
            Assert.AreEqual(expected, mapped);
        }

        [Test]
        public void CollectionOptionalStringFlatMapTest()
        {
            //Arrange
            var collection = new Optional<string>[] { "1", null, "2", "3", null, "4" };
            var expected = new int[] { 1, 2, 3, 4 };

            //Act
            var flatMapped = collection.FlatMap((x) => int.Parse(x));

            //Assert
            Assert.IsNotEmpty(flatMapped);
            Assert.AreEqual(4, flatMapped.Count);
            Assert.AreEqual(expected, flatMapped);
        }

        [Test]
        public void CollectionStringReduceTest()
        {
            //Arrange
            var collection = new string[] { "s", "t", "r", "i", "n", "g" };

            //Act
            var reduced = collection.Reduce("", (x, y) => x += y);

            //Assert
            Assert.AreEqual("string", reduced);
        }

        [Test]
        public void OptionalCollectionMapTest()
        {
            //Arrange
            var optionalInts = new int[] { 1, 2, 3, 4 }.AsOptional();
            IOptional<int>[] expected = new Optional<int>[] { 1, 2, 3, 4 };

            //Act
            var mapped = optionalInts.Map(x => x);

            //Assert
            Assert.IsNotEmpty(mapped);
            Assert.IsInstanceOf<IOptional<int>[]>(mapped);
        }

        [Test]
        public void OptionalCollectionFlatMapTest()
        {
            //Arrange
            var optionalInts = new int[] { 1, 2, 3, 4 }.AsOptional();
            IOptional<int>[] expected = new Optional<int>[] { 1, 2, 3, 4 };

            //Act
            var mapped = optionalInts.FlatMap(x => x);

            //Assert
            Assert.IsNotEmpty(mapped);
            Assert.IsInstanceOf<IOptional<int>[]>(mapped);
        }

        [Test]
        public void ListFlatMapIntToOptionalInt()
        {
            //Arrange
            var optionalInts = new int[] { 1, 2, 3, 4 };
            IOptional<int>[] expected = new Optional<int>[] { 1, 2, 3, 4 };

            //Act
            var mapped = optionalInts.FlatMap(x => (Optional<int>)x);

            //Assert
            Assert.IsNotEmpty(mapped);
            Assert.IsInstanceOf<IOptional<int>[]>(mapped);
        }

        [Test]
        public void OptionalCollectionOfOptionalCollectionTest()
        {
            //Arrange
            var optionalInts = new int[] { 1, 2, 3, 4, 5 };

            //Act & Assert
            Assert.Throws<OptionalCollectionWrapException>(() => optionalInts.AsOptional().AsOptional());
        }
    }
}