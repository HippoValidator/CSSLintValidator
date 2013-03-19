using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Jurassic;
using Jurassic.Library;

namespace HippoValidator.CssLintValidator
{
    public class CssLintValidator : ICssLintValidator
    {
        readonly ScriptEngine _scriptEngine;

        public CssLintValidator()
        {
            _scriptEngine = new ScriptEngine();
            _scriptEngine.CompatibilityMode = CompatibilityMode.ECMAScript3;
            var loadedScript = typeof(CssLintValidator).Assembly.GetManifestResourceStream("HippoValidator.CssLintValidator.Scripts.CSSLint.js");
            Debug.Assert(loadedScript != null, "CSSLint.js != null");
            using (var reader = new StreamReader(loadedScript))
            {
                _scriptEngine.Execute(reader.ReadToEnd());
            }
        }

        public ValidationResult Validate(string css, Options options)
        {
            var result = new ValidationResult();
            if (options == null) options = Options.AllTrue();

            css = css
                .Replace("'", "\"")
                .Replace(Environment.NewLine, "\\")
                .Replace("\n", "\\")
                .Trim(new[] {'\\', ' '});

            var js = "var css = '" + css + "';" +
                     "var options = {};" +
                     (options.AdjoiningClasses ? "options['adjoining-classes']=true;" : string.Empty) +
                     (options.EmptyRules ? "options['empty-rules']=true;" : string.Empty) +
                     (options.DisplayPropertyGrouping ? "options['display-property-grouping']=true;" : string.Empty) +
                     (options.Floats ? "options['floats']=true;" : string.Empty) +
                     (options.FontFaces ? "options['font-faces']=true;" : string.Empty) +
                     (options.Shorthand ? "options['shorthand']=true;" : string.Empty) +
                     (options.FontSizes ? "options['font-sizes']=true;" : string.Empty) +
                     (options.Ids ? "options['ids']=true;" : string.Empty) +
                     (options.QualifiedHeadings ? "options['qualified-headings']=true;" : string.Empty) +
                     (options.UniqueHeadings ? "options['unique-headings']=true;" : string.Empty) +
                     (options.ZeroUnits ? "options['zero-units']=true;" : string.Empty) +
                     (options.VendorPrefix ? "options['vendor-prefix']=true;" : string.Empty) +
                     (options.Gradients ? "options['gradients']=true;" : string.Empty) +
                     (options.RegexSelectors ? "options['regex-selectors']=true;" : string.Empty) +
                     (options.BoxModel ? "options['box-model']=true;" : string.Empty) +
                     (options.Import ? "options['import']=true;" : string.Empty) +
                     (options.Important ? "options['important']=true;" : string.Empty) +
                     (options.CompatibleVendorPrefixes ? "options['compatible-vendor-prefixes']=true;" : string.Empty) +
                     (options.DuplicateProperties ? "options['duplicate-properties']=true;" : string.Empty) +
                     "var result = CSSLint.verify(css, options);" + "var errors = result.messages;";

            _scriptEngine.Execute(js);

            var errors = ((ArrayInstance)_scriptEngine.GetGlobalValue("errors"))
                .ElementValues
                .OfType<ObjectInstance>();

            foreach (var error in errors.Where(error => error != null))
            {
                result.Errors.Add(new Error
                    {
                        Column = Get(error, "col", 0),
                        Line = Get(error, "line", 0),
                        Evidence = Get(error, "evidence", string.Empty),
                        Message = Get(error, "message", string.Empty),
                    });
            }

            return result;
        }

        private static T Get<T>(ObjectInstance dic, string name, T defaultValue)
        {
            var value = dic.GetPropertyValue(name);
            T ret = defaultValue;

            if (value is Undefined)
            {
                return ret;
            }
            
            try
            {
                if (defaultValue is string) ret = (T)(object)Convert.ToString(value);
                else ret = (T)Convert.ChangeType(value, typeof(T));
            }
            catch { }
            return ret;
        }
    }
}