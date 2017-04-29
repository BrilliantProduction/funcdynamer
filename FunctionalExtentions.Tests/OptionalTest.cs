using FunctionalExtentions.Core;
using NUnit.Framework;

namespace FunctionalExtentions.Tests
{
    [TestFixture]
    public class OptionalTest
    {
        [Test]
        public void OptionalCreateDefaultValueThrowsOptionalCastException()
        {
            //Arrange
            Optional<int> opt = Optional<int>.CreateOptional();

            //Act & Assert
            Assert.Throws<OptionalCastException>(() => { var a = opt.Value; });
        }

        [Test]
        public void OptionalCreateOptionalFromInt()
        {
            //Arrange
            Optional<int> opt = 1;

            //Act & Assert
            Assert.AreEqual(1, opt.Value);
        }
    }
}