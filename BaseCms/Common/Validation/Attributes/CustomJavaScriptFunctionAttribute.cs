using System;
using BaseCms.Common.Validation.Attributes.Base;

namespace BaseCms.Common.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomJavaScriptFunctionAttribute : ValidationAttribute
    {
        private string JavaScriptFunctionName { get; set; }

        public CustomJavaScriptFunctionAttribute(string javaScriptFunctionName, string errorMessage = "",
                               int order = 0)
            : base(errorMessage, order)
        {
            JavaScriptFunctionName = javaScriptFunctionName;
        }

        public override string Render()
        {
            return String.Format("jscriptfunc({0}),{1}", ErrorMessage, JavaScriptFunctionName);
        }
    }
}
