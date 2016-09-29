using System;
using System.ComponentModel;

namespace BackofficeDemo.Model.Enums
{
    [Flags]
    public enum PaymentType
    {
        [Description("Cash")]
        Cash = 2,
        [Description("Online by card")]
        ByCartOnline = 4,
        [Description("By card at delivery")]
        ByCartCourier = 8
    }
}
