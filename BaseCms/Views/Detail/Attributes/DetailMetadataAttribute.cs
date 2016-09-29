using System;

namespace BaseCms.Views.Detail.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DetailMetadataAttribute : Attribute
    {
        //Подход с публичными полями используется, чтобы обойти ограничения аттрибутов принимать Nullable типы
        public bool? IsEditableValue;
        public bool? IsKeyValue;
        public bool? IsRequiredValue;
        public bool? IsHiddenValue;
        public bool? IsPasswordValue;
        public int? OrderValue = Int32.MaxValue;

        public string Display { get; set; }
        public string Template { get; set; }
        public string Format { get; set; }

        public bool IsHeaderDisplayProperty { get; set; }

        public int MaxLength { get; set; }
        public int Tab { get; set; }
        public int Block { get; set; }

        public bool IsEditable
        {
            set { IsEditableValue = value; }
            get { throw new NotSupportedException(); }
        }

        public bool IsKey
        {
            set { IsKeyValue = value; }
            get { /*throw new NotSupportedException();*/
                return IsKeyValue.HasValue ? IsKeyValue.Value : false;
            }
        }
        public bool IsRequired
        {
            set { IsRequiredValue = value; }
            get { throw new NotSupportedException(); }
        }
        public bool IsHidden
        {
            set { IsHiddenValue = value; }
            get { throw new NotSupportedException(); }
        }
        public bool IsPassword
        {
            set { IsPasswordValue = value; }
            get { throw new NotSupportedException(); }
        }
        public int Order
        {
            set { OrderValue = value; }
            get { throw new NotSupportedException(); }
        }
    }
}
