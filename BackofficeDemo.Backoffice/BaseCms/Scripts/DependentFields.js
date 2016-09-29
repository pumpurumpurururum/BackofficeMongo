/// <reference path="typings/jquery/jquery.d.ts" />
// Module
var CMSViews;
(function (CMSViews) {
    // Class
    var DependentField = (function () {
        // Constructor
        function DependentField(x, y) {
            this.x = x;
            this.y = y;
        }
        DependentField.prototype.init = function (fieldId, dependentFromFields) {
            $.each(dependentFromFields, function (index, obj) {
                $('#' + dependentFromFields).change(function () {
                });
            });
        };

        DependentField.prototype.handle = function (data) {
        };
        return DependentField;
    })();
    CMSViews.DependentField = DependentField;

    var FieldData = (function () {
        function FieldData() {
        }
        return FieldData;
    })();
    CMSViews.FieldData = FieldData;
})(CMSViews || (CMSViews = {}));
//# sourceMappingURL=DependentFields.js.map
