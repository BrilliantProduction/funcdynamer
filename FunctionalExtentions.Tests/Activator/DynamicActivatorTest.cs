using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FunctionalExtentions.Tests.Activator
{
    [TestFixture]
    public class DynamicActivatorTest
    {
        [Test]
        public void MakeObject_CreateDateTime()
        {
            //Arrange & Act & Assert
            Assert.NotNull(DynamicActivator.MakeObject<DateTime>());
        }

        [Test]
        public void MakeObject_CreateDateTimeWithParameters()
        {
            //Arrange & Act
            var expected = new DateTime(2017, 11, 7);
            var date = DynamicActivator.MakeObjectWithParams<DateTime>(2017, 11, 7);

            //Assert
            Assert.AreEqual(expected, date);
        }

        [Test]
        public void MakeObject_CreateList()
        {
            //Arrange & Act
            var expected = new List<int>(10);
            var list = DynamicActivator.MakeObjectWithParams<List<int>>(10);

            //Assert
            Assert.AreEqual(expected.Capacity, list.Capacity);
        }

        [Test]
        public void MakeObject_CreateTuple()
        {
            //Arrange & Act
            var expected = new Tuple<int, int>(10, 10);
            var tuple = DynamicActivator.MakeObjectWithParams<Tuple<int, int>>(10, 10);

            //Assert
            Assert.AreEqual(expected, tuple);
        }

        [Test]
        public void MakeObject_CreateDictionary()
        {
            //Arrange & Act
            var expected = new Dictionary<string, int>(10, EqualityComparer<string>.Default);
            var dictionary = DynamicActivator.MakeObjectWithParams<Dictionary<string, int>>(10, EqualityComparer<string>.Default);

            //Assert
            Assert.AreEqual(expected.Comparer, dictionary.Comparer);
            Assert.AreEqual(expected, dictionary);
        }
    }
}