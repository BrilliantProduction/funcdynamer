using FunctionalExtentions.Core.UnWrap;
using NUnit.Framework;
using System;

namespace FunctionalExtentions.Tests
{
    [TestFixture]
    public class ScopeTest
    {
        [Test]
        public void ScopeAddIntAndGetInt()
        {
            //Arrange
            var scope = new Scope();
            int a = 25;
            scope.SetValue(nameof(a), 25);

            //Act
            var b = scope.GetValue<int>(nameof(a));

            //Assert
            Assert.AreEqual(a, b);
        }

        [Test]
        public void ScopeTreeSetIntAndGetInt()
        {
            //Arrange
            var parentScope = new Scope();
            var child1 = new Scope();
            var child2 = new Scope();
            parentScope.AddChildScope(child1);
            child1.AddChildScope(child2);
            int testA = 2228;

            //Act
            child1.SetValue(nameof(testA), testA);
            var testB = child2.GetValue<int>(nameof(testA));

            //Assert
            Assert.AreEqual(testA, testB);
        }

        [Test]
        public void ScopeTreeDontHaveElement()
        {
            //Arrange
            var scope = new Scope();
            for (int i = 0; i < 5; i++)
            {
                scope.AddChildScope(new Scope());
            }

            //Act & Assert
            Assert.Throws<Exception>(() => scope.GetValue<int>("testA"));
        }
    }
}