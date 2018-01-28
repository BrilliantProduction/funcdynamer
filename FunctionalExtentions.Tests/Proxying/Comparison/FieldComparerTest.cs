using NUnit.Framework;
using System;
using System.Collections.Generic;
using FunctionalExtentions.Tests.Dummies.Comparison;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionalExtentions.Proxying.Compability.Comparers;
using System.Reflection;

namespace FunctionalExtentions.Tests.Proxying.Comparison
{
    [TestFixture]
    class FieldComparerTest
    {
        public const BindingFlags All = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        [Test]
        public void FieldsComparer_FieldsAreEqual()
        {
            //Arrange & Act
            var comparer = new FieldCompatibilityComparer();
            var fieldA = typeof(ClassWithIntField).GetField("_a", All);
            var otherClassFieldA = typeof(AnotherClassWithIntField).GetField("_a", All);

            //Assert
            Assert.IsTrue(comparer.IsCompatible(fieldA, otherClassFieldA));
        }

        [Test]
        public void FieldsComparer_FieldsAreImplicitlyCastable()
        {
            //Arrange & Act
            var comparer = new FieldCompatibilityComparer();
            var fieldA = typeof(ClassWithIntField).GetField("_a", All);
            var otherClassFieldA = typeof(ClassWithDoubleField).GetField("_a", All);

            //Assert
            Assert.IsTrue(comparer.IsCompatible(fieldA, otherClassFieldA));
        }
    }
}
