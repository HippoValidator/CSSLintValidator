namespace HippoValidator.CssLintValidator
{
    public class Options
    {
        public bool AdjoiningClasses { get; set; }

        public bool EmptyRules { get; set; }

        public bool DisplayPropertyGrouping { get; set; }

        public bool Floats { get; set; }

        public bool FontFaces { get; set; }

        public bool Shorthand { get; set; }

        public bool FontSizes { get; set; }

        public bool Ids { get; set; }

        public bool QualifiedHeadings { get; set; }

        public bool UniqueHeadings { get; set; }

        public bool ZeroUnits { get; set; }

        public bool VendorPrefix { get; set; }

        public bool Gradients { get; set; }

        public bool RegexSelectors { get; set; }

        public bool BoxModel { get; set; }

        public bool Import { get; set; }

        public bool Important { get; set; }

        public bool CompatibleVendorPrefixes { get; set; }

        public bool DuplicateProperties { get; set; }

        public static Options AllTrue()
        {
            return new Options
                {
                    AdjoiningClasses = true,
                    BoxModel = true,
                    CompatibleVendorPrefixes = true,
                    DisplayPropertyGrouping = true,
                    DuplicateProperties = true,
                    EmptyRules = true,
                    Floats = true,
                    FontFaces = true,
                    FontSizes = true,
                    Gradients = true,
                    Ids = true,
                    Import = true,
                    Important = true,
                    QualifiedHeadings = true,
                    RegexSelectors = true,
                    Shorthand = true,
                    UniqueHeadings = true,
                    VendorPrefix = true,
                    ZeroUnits = true
                };
        }
    }
}