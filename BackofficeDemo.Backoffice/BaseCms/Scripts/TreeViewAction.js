/// <reference path="typings/jquery.form/jquery.form.d.ts" />
/// <reference path="typings/bootbox/bootbox.d.ts" />
/// <reference path="Views.ts" />
var CMSViews;
(function (CMSViews) {
    var TreeViewAction = (function () {
        function TreeViewAction(context, behavior, callback, caption, style, uniqueId, icon) {
            this._context = context;
            this._behavior = behavior;
            this._callback = callback;
            this._caption = caption;
            this._style = style;
            this._uniqueId = uniqueId;
            this._icon = icon;
        }
        TreeViewAction.prototype.render = function (container) {
            var icon = (this._icon == undefined || this._icon == '') ? '' : '<i class="' + this._icon + ' bigger-110"></i>';
            container.append('<button style="margin-left: 4px;" id="' + this._uniqueId + '" class="btn ' + this._style + '" type="button" >' + icon + this._caption + '</button>');
            var self = this;
            $("body").delegate("#" + this._uniqueId, "click", function () {
                self._callback.call(self._context);
            });
        };

        TreeViewAction.prototype.repaint = function (selectedItems, state) {
            this._behavior.repaint(selectedItems, state, $("#" + this._uniqueId));
        };
        return TreeViewAction;
    })();
    CMSViews.TreeViewAction = TreeViewAction;
})(CMSViews || (CMSViews = {}));
//# sourceMappingURL=TreeViewAction.js.map
