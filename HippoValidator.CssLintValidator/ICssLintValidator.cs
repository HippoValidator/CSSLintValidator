namespace HippoValidator.CssLintValidator
{
    public interface ICssLintValidator
    {
        ValidationResult Validate(string css, Options options);
    }
}