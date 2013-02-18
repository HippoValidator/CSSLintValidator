using System.Collections.Generic;

namespace HippoValidator.CssLintValidator
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<Error>();
        }

        public List<Error> Errors { get; set; }
    }
}