using System;
using BaseCms.Common.Validation.Attributes.Base;

namespace BaseCms.Common.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RegularExpressionAttribute : ValidationAttribute
    {
        public string Pattern { get; set; }
        public RegularExpressionAttribute(string pattern, string errorMessage = "", int order = 0)
            : base(errorMessage, order)
        {
            Pattern = pattern;
        }

        public override string Render()
        {
            return String.Format("regexp{0},{1}", RenderedErrorMessage, Pattern
                                                                            .Replace("\"", "{{quot}}")
                                                                            .Replace("|", "{{stick}}")
                                                                            .Replace(",", "{{comma}}"));
        }
    }
}
