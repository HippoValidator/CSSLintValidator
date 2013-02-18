using System.Linq;
using NUnit.Framework;

namespace HippoValidator.CssLintValidator.Tests
{
    public class CssLintValidatorTest
    {
        [Test]
        public void CanValidateValidCss()
        {
            // Arrange
            var validator = new CssLintValidator();

            // Act
            var result = validator.Validate("a { border: 1px; }");

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanValidateWithErrors()
        {
            // Arrange
            var validator = new CssLintValidator();

            // Act
            var result = validator.Validate(".foo {}");

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors.First().Column, Is.Not.EqualTo(0));
            Assert.That(result.Errors.First().Line, Is.Not.EqualTo(0));
            Assert.That(result.Errors.First().Evidence, Is.Not.Null);
            Assert.That(result.Errors.First().Message, Is.Not.Null);
        }
    }
}