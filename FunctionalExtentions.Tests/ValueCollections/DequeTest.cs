using FunctionalExtentions.ValueCollections.Queues;
using NUnit.Framework;
using System.Linq;

namespace FunctionalExtentions.Tests.ValueCollections
{
    [TestFixture]
    public class DequeTest
    {
        [Test]
        public void DequeRemoveTest_CountChanged()
        {
            //Arrange & Act
            var deque = new Deque<int>();
            Enumerable.Range(0, 10).ToList().ForEach(x => deque.Add(x));
            deque.Remove(5);
            //Assert
            Assert.AreEqual(9, deque.Count);
        }

        [Test]
        public void DequeAddTwo_EveryItemIcreasedByTwo()
        {
            //Arrange & Act
            var source = new int [] { 1, 2, 3, 4, 5 }.ToList();
            var deque = new Deque<int>();
            foreach (var item in source)
                deque.AddFirst(item + 2);

            //Assert
            for (int i = 0; i < source.Count; i++)
            {
                Assert.Greater(deque.RemoveLast(), source[i]);
            }
        }
    }
}
