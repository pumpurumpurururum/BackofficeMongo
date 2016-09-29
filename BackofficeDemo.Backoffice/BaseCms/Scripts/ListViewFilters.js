// <reference path="typings/jquery.form/jquery.form.d.ts" />
/// <reference path="bootbox.min.js" />
/// <reference path="Views.js" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var CMSViews;
(function (CMSViews) {
    var Filter = (function () {
        function Filter(id, caption) {
            this._id = id;
            this._caption = caption;
        }
        Filter.prototype.render = function (context) {
        };
        Filter.prototype.createContainer = function (rootElement) {
            var container = rootElement.find("div#filter");
            container.append("<div id='container-for-" + this._id + "' class='pull-left filter-container form-group'></div>");
            return rootElement.find("div#container-for-" + this._id);
        };
        return Filter;
    })();
    CMSViews.Filter = Filter;

    var CollectionFilter = (function (_super) {
        __extends(CollectionFilter, _super);
        function CollectionFilter(id, caption, collectionName, isPreloaded, emptyValueText, emptyValue, relatedListId) {
            this._isPreloaded = isPreloaded;
            this._collectionName = collectionName;
            this._emptyValueText = emptyValueText;
            this._emptyValue = emptyValue;
            this._relatedListId = relatedListId;
            _super.call(this, id, caption);
        }
        CollectionFilter.prototype.render = function (context) {
            var rootElement = context._rootElement;
            var collection = this._collectionName;
            var caption = encodeURIComponent(this._caption);
            var id = this._id;
            var isPreloaded = this._isPreloaded;
            var relListId = this._relatedListId;
            var emptyValueText = '';
            if (this._emptyValueText != undefined) {
                emptyValueText = "&emptyValueText=" + encodeURIComponent(this._emptyValueText);
            }

            var emptyValue = '';
            if (this._emptyValue != undefined) {
                emptyValue = "&emptyValue=" + encodeURIComponent(this._emptyValue);
            }

            var relatedValue = '';
            if (relListId != undefined) {
                
                relatedValue = "&relatedValue=" + relListId;
            }

            var container = this.createContainer(rootElement);

            $.ajax({
                url: "/List/CollectionFilter?id=" + id + "&collection=" + collection + "&caption=" + caption + "&isPreloaded=" + isPreloaded + emptyValueText + emptyValue + relatedValue,
                type: 'POST',
                dataType: 'html',
                async: false,
                success: function (html) {
                    container.append(html);
                }
            });
        };
        return CollectionFilter;
    })(Filter);
    CMSViews.CollectionFilter = CollectionFilter;

    var StringPropertyFilter = (function (_super) {
        __extends(StringPropertyFilter, _super);
        function StringPropertyFilter(id, caption) {
            _super.call(this, id, caption);
        }
        StringPropertyFilter.prototype.render = function (context) {
            var rootElement = context._rootElement;
            var caption = encodeURIComponent(this._caption);
            var id = this._id;

            var container = this.createContainer(rootElement);

            $.ajax({
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                url: "/List/StringPropertyFilter?&id=" + id + "&caption=" + caption,
                type: 'POST',
                dataType: 'html',
                async: false,
                success: function (html) {
                    container.append(html);
                }
            });
        };
        return StringPropertyFilter;
    })(Filter);
    CMSViews.StringPropertyFilter = StringPropertyFilter;

    var StringPropertyFilterText = (function (_super) {
        __extends(StringPropertyFilterText, _super);
        function StringPropertyFilterText(id, caption) {
            _super.call(this, id, caption);
        }
        StringPropertyFilterText.prototype.render = function (context) {
            var rootElement = context._rootElement;
            var caption = encodeURIComponent(this._caption);
            var id = this._id;

            var container = this.createContainer(rootElement);

            $.ajax({
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                url: "/List/StringPropertyFilterText?&id=" + id + "&caption=" + caption,
                type: 'POST',
                dataType: 'html',
                async: false,
                success: function (html) {
                    container.append(html);
                }
            });
        };
        return StringPropertyFilter;
    })(Filter);
    CMSViews.StringPropertyFilterText = StringPropertyFilterText;

    var DateTimePropertyFilter = (function (_super) {
        __extends(DateTimePropertyFilter, _super);
        function DateTimePropertyFilter(id, caption, defaultValue) {
            this._defaultValue = defaultValue;
            _super.call(this, id, caption);
        }
        DateTimePropertyFilter.prototype.render = function (context) {
            var rootElement = context._rootElement;
            var id = this._id;
            var caption = encodeURIComponent(this._caption);

            var defaultValue = '';
            if (this._defaultValue != undefined) {
                defaultValue = "&defaultValue=" + this._defaultValue;
            }

            var container = this.createContainer(rootElement);

            $.ajax({
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                url: "/List/DateTimePropertyFilter?&id=" + id + "&caption=" + caption + defaultValue,
                type: 'POST',
                dataType: 'html',
                async: false,
                success: function (html) {
                    container.append(html);
                }
            });
        };
        return DateTimePropertyFilter;
    })(Filter);
    CMSViews.DateTimePropertyFilter = DateTimePropertyFilter;

    var DateRangePropertyFilter = (function (_super) {
        __extends(DateRangePropertyFilter, _super);
        function DateRangePropertyFilter(id, caption) {
            
            _super.call(this, id, caption);
        }
        DateRangePropertyFilter.prototype.render = function (context) {
            var rootElement = context._rootElement;
            var id = this._id;
            var caption = encodeURIComponent(this._caption);

            var container = this.createContainer(rootElement);

            $.ajax({
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                url: "/List/DateRangePropertyFilter?&id=" + id + "&caption=" + caption,
                type: 'POST',
                dataType: 'html',
                async: false,
                success: function (html) {
                    container.append(html);
                }
            });
        };
        return DateRangePropertyFilter;
    })(Filter);
    CMSViews.DateRangePropertyFilter = DateRangePropertyFilter;

    var CheckBoxPropertyFilter = (function (_super) {
        __extends(CheckBoxPropertyFilter, _super);
        function CheckBoxPropertyFilter(id, caption) {
            _super.call(this, id, caption);
        }
        CheckBoxPropertyFilter.prototype.render = function (context) {
            var rootElement = context._rootElement;
            var caption = encodeURIComponent(this._caption);
            var id = this._id;

            var container = this.createContainer(rootElement);

            $.ajax({
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                url: "/List/CheckBoxPropertyFilter?&id=" + id + "&caption=" + caption,
                type: 'POST',
                dataType: 'html',
                async: false,
                success: function (html) {
                    container.append(html);
                }
            });
        };
        return CheckBoxPropertyFilter;
    })(Filter);
    CMSViews.CheckBoxPropertyFilter = CheckBoxPropertyFilter;
})(CMSViews || (CMSViews = {}));
//# sourceMappingURL=ListViewFilters.js.map
