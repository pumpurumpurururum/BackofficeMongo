using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCms.Common.Validation.Attributes.Base;

namespace BaseCms.Views.Detail
{
    public static class BootstrapValidationHelper
    {
        public static string GetValidationHtmlAttribute(List<ValidationAttribute> attributes)
        {
            string result = String.Empty;
            if (attributes != null)
            {
                attributes = attributes.OrderBy(p => p.Order).ToList();
                foreach (var validation in attributes)
                {
                    result += validation.Render();
                    if (!validation.Equals(attributes.Last()))
                    {
                        result += "|";
                    }
                }
            }
            return result;
        }
    }
}
