using System.Linq;
using NUnit.Framework;

namespace HippoValidator.CssLintValidator.Tests
{
    public class CssLintValidatorTest
    {
        CssLintValidator _validator;

        [SetUp]
        public void SetUpValidator()
        {
            _validator = new CssLintValidator();
        }

        [Test]
        public void CanValidateValidCss()
        {
            // Arrange
            var css = @"
                a {
                    border: 1px;
                }";
            var options = Options.AllTrue();
            
            // Act
            var result = _validator.Validate(css, options);

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanDisableRule()
        {
            // Arrange
            var css = ".foo {}";
            var options = Options.AllTrue();
            options.EmptyRules = false;

            // Act
            var result = _validator.Validate(css, options);

            // Assert
            Assert.That(!result.Errors.Any());
        }

        [Test]
        public void CanValidateWithErrors()
        {
            // Arrange
            var css = @"
                .foo {
                }
            ";
            var options = new Options { EmptyRules = true };

            // Act
            var result = _validator.Validate(css, options);

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors.First().Column, Is.Not.EqualTo(0));
            Assert.That(result.Errors.First().Line, Is.Not.EqualTo(0));
            Assert.That(result.Errors.First().Evidence, Is.Not.Null);
            Assert.That(result.Errors.First().Message, Is.Not.Null);
        }

        [Test]
        public void CanValidateCssWithNewlines()
        {
            // Arrange
            var css = @"
                a {
                    border: 1px;
                }";
            var options = Options.AllTrue();

            // Act
            var result = _validator.Validate(css, options);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void CanValidateCssWithNewlineChar()
        {
            // Arrange
            var css = @"
                a {
                    border: 1px;\n}";
            var options = Options.AllTrue();

            // Act
            var result = _validator.Validate(css, options);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void CanValidateStyleWithApostrophe()
        {
            // Arrange
            var css = @"
                a {
                    font-family: 'Arvo', Georgia, Times, serif;
                }
            ";
            var options = Options.AllTrue();

            // Act
            var result = _validator.Validate(css, options);

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}