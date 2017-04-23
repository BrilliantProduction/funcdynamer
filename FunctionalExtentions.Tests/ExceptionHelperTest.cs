using NUnit.Framework;
using System;

namespace FunctionalExtentions.Tests
{
    [TestFixture]
    public class ExceptionHelperTest
    {
        [Test]
        public void ConvertExceptionToAriphmeticException()
        {
            //Arrange
            var converter = ExceptionTransformer<Exception, ArithmeticException>.MakeTransformer((e) => new ArithmeticException());
            Action unsafeAction = () => { throw new Exception(); };

            //Act & Assert
            Assert.Throws<ArithmeticException>(() => ExceptionHelper.HandleUnsafeAction(unsafeAction, converter));
        }
    }
}