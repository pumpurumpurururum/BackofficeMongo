using System;

namespace BaseCms.Views.ActionButtons
{
    public class ActionButton : ICloneable
    {
        public string Caption { get; set; }
        public string Class { get; set; }
        public string CMSScopeMethod { get; set; }
        public string Icon { get; set; }

        public bool SaveButton { get; set; }

        public object Clone()
        {
            return new ActionButton
            {
                Caption = Caption,
                Class = Class,
                CMSScopeMethod = CMSScopeMethod,
                Icon = Icon,
                SaveButton = SaveButton
            };
        }
    }
}
