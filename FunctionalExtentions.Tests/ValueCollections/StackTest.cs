using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Tests.ValueCollections
{
    [TestFixture]
    class StackTest
    {
        [Test]
        public void StackEnumeratorTest()
        {
            //Arrange && Act
            var testArray = new int[] { 1, 2, 3, 4, 5 };
            var myStack = new FunctionalExtentions.ValueCollections.Stack<int>();
            var systemStack = new Stack<int>();

            foreach (var item in testArray)
            {
                myStack.Push(item);
                systemStack.Push(item);
            }


            //Assert
            Assert.AreEqual(string.Join(", ", systemStack), string.Join(", ", myStack));
        }
    }
}
