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

        [Test]
        public void CanValidateLargeScript()
        {
            // Arrange
            var css = @"
                body{
	                background: #eee url('grain.png') 0 0;
	                color: #222;
	                font-family: 'Arvo', Georgia, Times, serif;
	                line-height: 1.5em;
	            }

                ::selection, 
                ::-moz-selection, 
                img::selection, 
                img::-moz-selection{
	                background: #2e7565;
	                background: rgba(46,117,101,.75);
	                color: #fff;
	                text-shadow: 1px 1px 0 rgba(0,0,0,.5);
	                }

                hr{
	                background: #2e7565;
                    background: -webkit-gradient(linear, 0 0, 100% 0, from(white), to(white), color-stop(50%, #2e7565));
	                display: block;
	                border: none;
	                color: white;
	                height: 1px;
	                margin: 1em 0;
	                }

                section{
	                background: #fff;
	                border: 1px solid #ccc;
	                border-radius: 5px;
	                display: block;
	                margin: 100px auto 0;
	                padding: 1em 2em 2em 2em;
	                position: relative;
	                width: 430px;
	                z-index: 100;

	                -moz-box-shadow: 2px 2px 30px rgba(0,0,0,.2);
	                -webkit-box-shadow: 2px 2px 30px rgba(0,0,0,.2);
	                box-shadow: 2px 2px 30px rgba(0,0,0,.2);
	                }
            ";

            // Act
            var result = _validator.Validate(css, Options.AllTrue());

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}