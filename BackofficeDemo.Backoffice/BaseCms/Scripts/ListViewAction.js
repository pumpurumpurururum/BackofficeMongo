///// <reference path="typings/jquery.form/jquery.form.d.ts" />
///// <reference path="typings/bootbox/bootbox.d.ts" />
///// <reference path="Views.ts" />
/// <reference path="typings/jquery.form/jquery.form.d.ts" />
/// <reference path="typings/bootbox/bootbox.d.ts" />
/// <reference path="Views.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var CMSViews;
(function (CMSViews) {
    var ListViewAction = (function () {
        function ListViewAction(context, behavior, callback, caption, style, uniqueId, icon) {
            this._context = context;
            this._behavior = behavior;
            this._callback = callback;
            this._caption = caption;
            this._style = style;
            this._uniqueId = uniqueId;
            this._icon = icon;
        }
        ListViewAction.prototype.render = function (container) {
            var icon = (this._icon == undefined || this._icon == '') ? '' : '<i class="' + this._icon + ' bigger-110"></i>';
            container.append('<button style="margin-left: 4px;" id="' + this._uniqueId + '" class="btn ' + this._style + '" type="button" >' + icon + this._caption + '</button>');
            var self = this;
            $("body").delegate("#" + this._uniqueId, "click", function () {
                self._callback.call(self._context);
            });
        };

        ListViewAction.prototype.repaint = function (selectedItems, state) {
            this._behavior.repaint(selectedItems, state, $("#" + this._uniqueId));
        };
        return ListViewAction;
    })();
    CMSViews.ListViewAction = ListViewAction;

    var NestedListViewAction = (function (_super) {
        __extends(NestedListViewAction, _super);
        function NestedListViewAction(context, behavior, callback, caption, style, uniqueId, subactions) {
            this._subactions = subactions;
            _super.call(this, context, behavior, callback, caption, style, uniqueId, "");
        }
        NestedListViewAction.prototype.render = function (container) {
            var html = '<div class="btn-group"><button id=' + this._uniqueId + ' class="btn ' + this._style + '">' + this._caption + '</button><button class="btn dropdown-toggle ' + this._style + '" data-toggle="dropdown"><span class="caret"></span></button><ul class="dropdown-menu">';
            for (var i = 0; i < this._subactions.length; i++) {
                html += '<li><a id=' + this._subactions[i]._uniqueId + ' href="#">' + this._subactions[i]._caption + '</a></li>';
            }
            html += '</ul></div>';
            container.append(html);

            var listViewActions = this._subactions;
            listViewActions.push(this);
            for (var i = 0; i < listViewActions.length; i++) {
                var regFunc = function (index) {
                    $("body").delegate("#" + listViewActions[index]._uniqueId, "click", function () {
                        listViewActions[index]._callback.call(listViewActions[index]._context);
                    });
                };
                regFunc(i);
            }
        };

        NestedListViewAction.prototype.repaint = function (selectedItems, state) {
            this._behavior.repaint(selectedItems, state, $("#" + this._uniqueId));
        };
        return NestedListViewAction;
    })(ListViewAction);
    CMSViews.NestedListViewAction = NestedListViewAction;

    var CreateActionBehavior = (function () {
        function CreateActionBehavior() {
        }
        CreateActionBehavior.prototype.repaint = function (selectedItems, state, element) {
        };
        return CreateActionBehavior;
    })();
    CMSViews.CreateActionBehavior = CreateActionBehavior;

    var RemoveActionBehavior = (function () {
        function RemoveActionBehavior() {
        }
        RemoveActionBehavior.prototype.repaint = function (selectedItems, state, element) {
            if (selectedItems.length > 0 || state) {
                element.removeClass("disabled");
                element.removeAttr("disabled");
            } else {
                element.addClass("disabled");
                element.attr("disabled", "disabled");
            }
        };
        return RemoveActionBehavior;
    })();
    CMSViews.RemoveActionBehavior = RemoveActionBehavior;

    var EditActionBehavior = (function () {
        function EditActionBehavior() {
        }
        EditActionBehavior.prototype.repaint = function (selectedItems, state, element) {
            if (selectedItems.length == 1 && !state) {
                element.removeClass("disabled");
                element.removeAttr("disabled");
            } else {
                element.addClass("disabled");
                element.attr("disabled", "disabled");
            }
        };
        return EditActionBehavior;
    })();
    CMSViews.EditActionBehavior = EditActionBehavior;

    var RefreshActionBehavior = (function () {
        function RefreshActionBehavior() {
        }
        RefreshActionBehavior.prototype.repaint = function (selectedItems, state, element) {
        };
        return RefreshActionBehavior;
    })();
    CMSViews.RefreshActionBehavior = RefreshActionBehavior;

    var UniteCompaniesActionBehavior = (function () {
        function UniteCompaniesActionBehavior() {
        }
        UniteCompaniesActionBehavior.prototype.repaint = function (selectedItems, state, element) {
            if (selectedItems.length > 1 || state) {
                element.removeClass("disabled");
                element.removeAttr("disabled");
            } else {
                element.addClass("disabled");
                element.attr("disabled", "disabled");
            }
        };
        return UniteCompaniesActionBehavior;
    })();
    CMSViews.UniteCompaniesActionBehavior = UniteCompaniesActionBehavior;
})(CMSViews || (CMSViews = {}));
//# sourceMappingURL=ListViewAction.js.map
