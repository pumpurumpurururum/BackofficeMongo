using System;

namespace BaseCms.Common.Validation.Attributes.Base
{
    public abstract class ValidationAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public int Order { get; set; }
        protected string RenderedErrorMessage
        {
            get
            {
                return !String.IsNullOrEmpty(ErrorMessage)
                           ? String.Format("({0})", ErrorMessage)
                           : String.Empty;
            }
        }


        protected ValidationAttribute(string errorMessage, int order = 0)
        {
            ErrorMessage = errorMessage;
            Order = order;
        }

        public abstract string Render();
    }
}
