/// <reference path="../TeslaCMS/Scripts/typings/jquery/jquery.d.ts" />
// /// <reference path="../TeslaCMS/Scripts/Views.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var CMSViews;
(function (CMSViews) {
    var BillingTransactionView = (function (_super) {
        __extends(BillingTransactionView, _super);
        function BillingTransactionView() {
            _super.apply(this, arguments);
        }
        BillingTransactionView.prototype.getUrl = function () {
            console.log("x");
            return "BillingTransaction/Index";
        };
        return BillingTransactionView;
    })(CMSViews.CMSViewBase);
    CMSViews.BillingTransactionView = BillingTransactionView;
})(CMSViews || (CMSViews = {}));
