namespace HippoValidator.CssLintValidator
{
    public class Warning
    {
        public int Line { get; set; }

        public int Column { get; set; }

        public string Evidence { get; set; }

        public string Message { get; set; }
    }
}