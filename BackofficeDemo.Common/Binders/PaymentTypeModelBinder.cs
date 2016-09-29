using System.Linq;
using System.Web.Mvc;
using BackofficeDemo.Model.Enums;


namespace BackofficeDemo.Common.Binders
{
    public class PaymentTypeModelBinder : DefaultModelBinder
    {



        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var rawValues = (value?.RawValue as string[])?.Select(x => (PaymentType)int.Parse(x));
            if (rawValues != null)
            {

                var result = rawValues.Aggregate((i, t) => i | t);
                return result;

                //if (Enum.TryParse(string.Join(",", rawValues), out result))
                //{
                //    return result;
                //}
                //       var ret = rawValues.Select((PaymentType)is_valid_value_for_conversion)
                //return 

            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
