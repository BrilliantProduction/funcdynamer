﻿using NUnit.Framework;

using FunctionalExtentions.Tests.Dummies.Comparison;
using FunctionalExtentions.Proxying.Compability.Comparers;

namespace FunctionalExtentions.Tests.Proxying.Comparison
{
    [TestFixture]
    class MethodComparerTest
    {
        [Test]
        public void MethodsCompare_MethodsAreEqual()
        {
            //Arrange
            var sut = new MethodCompatibilityComparer();
            var firstMethod = typeof(MethodWithInt).GetMethod("GetInt");
            var secondMethod = typeof(MethodWithDouble).GetMethod("GetInt");

            //Act
            var result = sut.IsCompatible(firstMethod, secondMethod);

            //Assert
            Assert.IsTrue(result);
        }
    }
}