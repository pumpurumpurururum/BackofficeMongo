using System.ComponentModel;

namespace BackofficeDemo.Model.Enums
{
    public enum GroupSelectionType
    {

        [Description("Multiple values")]
        CheckBoxes = 0,
        [Description("Single value")]
        RadioButtons = 10
    }
}
