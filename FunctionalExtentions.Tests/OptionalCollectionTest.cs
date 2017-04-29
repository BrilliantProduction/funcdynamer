using FunctionalExtentions.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}