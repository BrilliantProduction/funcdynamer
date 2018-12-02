using FunctionalExtentions.Collections.Trees;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Tests.ValueCollections
{
    [TestFixture]
    public class BinaryTreeTest
    {

        [Test]
        public void BinaryTree_GetEnumerator_ReturnsSameSequence()
        {
            //Arrange
            var binaryTree = new BinarySearchTree<int, string>();
            for (int i = 0; i < 10; i++)
            {
                binaryTree.Put(i, i.ToString());
            }
            var range = Enumerable.Range(0, 10).Select(x => new KeyValuePair<int, string>(x, x.ToString())).ToList();

            //Act
            var list = new List<KeyValuePair<int, string>>(binaryTree);

            //Assert
            Assert.AreEqual(range, list);
        }
    }
}
