using System;
using BaseCms.Common.Validation.Attributes.Base;

namespace BaseCms.Common.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : ValidationAttribute
    {
        public RequiredAttribute(string errorMessage = "Поле не может быть пустым!", int order = 0) : base(errorMessage, order) { }
        public override string Render()
        {
            return "required" + RenderedErrorMessage;
        }
    }
}
