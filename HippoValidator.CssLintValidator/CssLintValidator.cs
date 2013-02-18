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
            var loadedScript = typeof(CssLintValidator).Assembly.GetManifestResourceStream("HippoValidator.CssLintValidator.Scripts.CSSLint.js");
            Debug.Assert(loadedScript != null, "CSSLint.js != null");
            using (var reader = new StreamReader(loadedScript))
            {
                _scriptEngine.Execute(reader.ReadToEnd());
            }
        }

        public ValidationResult Validate(string css)
        {
            var result = new ValidationResult();

            _scriptEngine.Execute(
                //"var options = {};" +
                //"options['adjoining-classes']=true;" +
                //"options['empty-rules']=true;" +
                //"options['display-property-grouping']=true;" +
                //"options['floats']=true;" +
                //"options['font-faces']=true;" +
                //"options['font-sizes']=true;" +
                //"options['font-sizes']=true;" +
                //"options['ids']=true;" +
                //"options['qualified-headings']=true;" +
                //"options['unique-headings']=true;" +
                //"options['zero-units']=true;" +
                //"options['vendor-prefix']=true;" +
                //"options['gradients']=true;" +
                //"options['regex-selectors']=true;" +
                //"options['box-model']=true;" +
                //"options['import']=true;" +
                //"options['important']=true;" +
                //"options['compatible-vendor-prefixes']=true;" +
                //"options['duplicate-properties']=true;" +
                "var result = CSSLint.verify('" + css + "');" +
                "var errors = result.messages;");
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