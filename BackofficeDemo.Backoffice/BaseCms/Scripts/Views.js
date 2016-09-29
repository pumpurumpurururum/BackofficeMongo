/// <reference path="ListViewAction.js" />
/// <reference path="TreeViewAction.js" />
/// <reference path="ListViewFilters.js" />
/// <reference path="typings/jquery.form/jquery.form.js" />
/// <reference path="typings/dataTables/dataTables.js" />
/// <reference path="typings/treeView/treeView.js" />
/// <reference path="typings/tinyMCE/tinyMCE.js" />
/// <reference path="typings/bootbox/bootbox.js" />
/// <reference path="typings/browserDetection/browserDetection.js" />
/// <reference path="typings/codeMirror/codeMirror.js" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var CMSViews;
(function (CMSViews) {
    var Helper = (function () {
        function Helper() {
        }
        Helper.getNewScopeName = function () {
            return "scope" + (this._scopeIndex++);
        };
        Helper.Get = function (scopeName) {
            return this._scopes[scopeName];
        };

        Helper.Set = function (scopeName, view) {
            this._scopes[scopeName] = view;
        };
        Helper._scopeIndex = 0;
        Helper._scopes = new Array();
        return Helper;
    })();
    CMSViews.Helper = Helper;

    var CMSViewBase = (function () {
        function CMSViewBase() {
            this._scopeName = Helper.getNewScopeName();
            this.setInnerHtml($.ajax({ url: this.getUrl(), context: this, async: false }).responseText);
            Helper.Set(this._scopeName, this);
        }
        CMSViewBase.prototype.assign = function (parentView, element) {
            this._rootElement = element;
            this._parentView = parentView;
            this._rootElement.html(this._innerHtml);
            this.init();
        };

        CMSViewBase.prototype.setInnerHtml = function (data) {
            if (data == "timeout")
                window.location.href = '/login/?ReturnUrl=%2f';
else
                this._innerHtml = data.replace(/_CMSScope_/g, "CMSViews.Helper.Get('" + this._scopeName + "')").replace(/_CMSScopeName_/g, this._scopeName);
        };

        CMSViewBase.prototype.init = function () {
        };

        CMSViewBase.prototype.getUrl = function () {
            return "";
        };
        return CMSViewBase;
    })();
    CMSViews.CMSViewBase = CMSViewBase;

    var Tab = (function () {
        function Tab(name, childView, menu) {
            this._id = childView._scopeName + "_tab";
            this._name = name;
            this._menu = menu;

            menu._tabMaxId++;

            var specPadding = "style='padding-right:10px;'";
            if (browserDetection.isMac()) {
                specPadding = "style='padding-top: 4px;padding-right:10px;'";
            }

            this._headerElement = $("<li id='" + this._id + "'><button onclick='CMSViews.Helper.Get(\"" + menu._scopeName + "\").closeTab(\"" + this._id + "\");' type=\"button\" id=\"close\" class=\"close\"" + specPadding + ">&times;</button><a href='#tab" + menu._tabMaxId + "' data-toggle='tab'>" + name + "</a></li>");
            this._contentElement = $("<div class=\"tab-pane\" id=\"tab" + menu._tabMaxId + "\"></div>");

            this.checkTabElements();
            menu._navigationTabsElement.append(this._headerElement);
            menu._contentElement.append(this._contentElement);

            childView.assign(menu, this._contentElement);
            childView._tab = this;

            var that = this;
            this._headerElement.on("shown", function (e) {
                HistoryKeeper.getInstance().push(that);
            });
        }
        Tab.prototype.checkTabElements = function () {
            //console.log(this._menu._rootElement.find('div.col-xs-12'));
            if (this._menu._rootElement.find('div.tabbable').length < 1) {
                this._menu._rootElement.find('div.page-content').find('div.col-xs-12').first().append('<div class="tabbable"><ul class="nav nav-tabs padding-18 tab-color-blue background-blue"></ul><div class="tab-content"></div></div>');
                //this._menu._rootElement.find('div.row').children().first().append('<div class="tabbable"><ul class="nav nav-tabs padding-18 tab-color-blue background-blue"></ul><div class="tab-content"></div></div>');
                this._menu._navigationTabsElement = this._menu._rootElement.find("ul.nav-tabs");
                this._menu._contentElement = this._menu._rootElement.find("div.tab-content");
            }
        };

        Tab.prototype.activate = function () {
            this._menu._navigationTabsElement.children("li.active").removeClass("active");
            //this._menu._navigationTabsElement.children("li.active").removeClass("active");
           // this._menu._contentElement.children("div.active").removeClass("active");
            this._menu._contentElement.children("div.active").removeClass("active");
            this._headerElement.addClass("active").addClass(this._name);
            this._contentElement.addClass("active").addClass(this._name);
        };

        Tab.prototype.isActive = function () {
            return this._headerElement.hasClass("active");
        };

        Tab.prototype.remove = function () {
            this._headerElement.remove();
            this._contentElement.remove();

            var ind = 0;
            for (var i = 0; i < this._menu._tabs.length; i++) {
                if (this._menu._tabs[i]._id == this._id) {
                    ind = i;
                    break;
                }
            }
            this._menu._tabs.splice(ind, 1);
            if (this._menu._tabs.length == 0) {
                this._menu._navigationTabsElement.parent().remove();
            }

            var hk = HistoryKeeper.getInstance();
            hk.removeFromHistory(this);

            if (this._menu._tabs.length > 0 && this.isActive()) {
                if (this._menu._tabs.length > ind) {
                    this._menu._tabs[ind].activate();
                    hk.push(this._menu._tabs[ind]);
                } else {

                    var str = this._name;
                    var a = str.split("#")[0];
                    var to = a.length - 1;
                    str = a.substring(0, to);

                   // this._menu._navigationTabsElement.children("li.active").removeClass("active");
                    //this._menu._navigationTabsElement.children("li.active").removeClass("active");
                    // this._menu._contentElement.children("div.active").removeClass("active");
                   // this._menu._contentElement.children("div.active").removeClass("active");

                   // this._headerElement.addClass("active").addClass(this._name);
                   // this._contentElement.addClass("active").addClass(this._name);
                   // $(this._menu._contentElement.find('li.' + str)).addClass("active");
                   // $(this._menu._headerElement.find('li.' + str)).addClass("active");

                   // this._menu._tabs[ind - 1].activate();
                    //this._menu._navigationTabsElement.parent().activate();
                    
                    for (var j = 0; j < this._menu._tabs.length; j++) {
                        if (this._menu._tabs[j]._name == str) {
                            ind = j;
                        }
                    }
                    if (ind == this._menu._tabs.length) {
                        this._menu._tabs[ind-1].activate();
                        hk.push(this._menu._tabs[ind-1]);
                    } else {
                        this._menu._tabs[ind].activate();
                        hk.push(this._menu._tabs[ind]);
                    }
                    
                    //hk.push(this._menu._tabs[ind - 1]);
                    
                }
            }
        };

        Tab.prototype.changeName = function (name) {
            this._headerElement.find("a").html(name);
            this._name = name;
        };
        return Tab;
    })();
    CMSViews.Tab = Tab;

    var MenuWithTabs = (function (_super) {
        __extends(MenuWithTabs, _super);
        function MenuWithTabs(collectionName) {
            _super.call(this);
            this.collectionName = collectionName;
            this._tabMaxId = 0;
            this._tabs = [];
        }
        MenuWithTabs.prototype.createTab = function (name, childView) {
            var tab = new Tab(name, childView, this);
            this._tabs.push(tab);
            this.activateTab(tab._name);
        };

        MenuWithTabs.prototype.createOrActivateTab = function (name, view) {
            if ($.grep(this._tabs, function (e) {
                return e._name == name;
            }).length == 0)
                this.createTab(name, view);
else {
                this.activateTab(name);
            }

            var oddHeight = $('div.navbar').height() + this._contentElement.position().top + 34 + 2;

            // 34 - tab-content div: border-top + border-bottom + padding-top + padding-bottom
            // 2 - page-content div: padding-bottom
            this._contentElement.css('min-height', ($(document).height() - oddHeight) + 'px');
        };

        MenuWithTabs.prototype.activateTab = function (name) {
            var tab = $.grep(this._tabs, function (e) {
                return e._name == name;
            })[0];
            tab.activate();
            HistoryKeeper.getInstance().push(tab);
        };

        MenuWithTabs.prototype.init = function () {
        };

        MenuWithTabs.prototype.getUrl = function () {
            return "MenuWithTabs/Read";
        };

        MenuWithTabs.prototype.closeTab = function (id) {
            var tab = $.grep(this._tabs, function (e) {
                return e._id == id;
            })[0];

            var detGuidInput = $(tab._contentElement).find("#detailViewGuid");
            if (detGuidInput[0] != undefined) {
                var collectionNameInput = $(tab._contentElement).find("#collectionName");
                $.ajax('/Detail/ClearDetailViewData/?collectionName=' + collectionNameInput.val() + '&detailViewGuid=' + detGuidInput.val());
            }

            tab.remove();
        };
        return MenuWithTabs;
    })(CMSViewBase);
    CMSViews.MenuWithTabs = MenuWithTabs;

    var SelectedItem = (function () {
        function SelectedItem(id, cmstype) {
            this.Id = id;
            this.Cmstype = cmstype;
        }
        return SelectedItem;
    })();

    


    CMSViews.SelectedItem = SelectedItem;

    var CmstypeCollectionAssociation = (function () {
        function CmstypeCollectionAssociation(collectionName, cmstype) {
            this.CollectionName = collectionName;
            this.Cmstype = cmstype;
        }
        return CmstypeCollectionAssociation;
    })();
    CMSViews.CmstypeCollectionAssociation = CmstypeCollectionAssociation;

    var ListView = (function (_super) {
        __extends(ListView, _super);
        function ListView(collectionName, createTabMethod, subcollections, filters, permissions, initFilters) {
            this._createTabMethod = createTabMethod;
            this._collectionName = collectionName;
            this._selectedAll = false;
            this._selectedItems = [];
            this._actions = [];
            this._filters = filters != null ? filters : [];

            this._viewMethod = 'List/Index';
            this._initFilters = initFilters;

            this._subcollections = subcollections;

            this.fillActions(permissions);

            _super.call(this);
        }
        ListView.prototype.fillActions = function (permissions) {
            var _this = this;
            var insertPerm = (permissions != null) && (permissions.length > 0) && (permissions[0]);
            var updatePerm = (permissions != null) && (permissions.length > 1) && (permissions[1]);
            var deletePerm = (permissions != null) && (permissions.length > 2) && (permissions[2]);

            if (this._subcollections && this._subcollections.length != 0) {
                var subactions = new Array();
                for (var i = 0; i < this._subcollections.length; i++) {
                    var register = function (assoc) {
                        subactions.push(new CMSViews.ListViewAction(_this, new CMSViews.CreateActionBehavior(), function () {
                            _this.createItem(assoc.CollectionName);
                        }, assoc.CollectionName, "btn-success", assoc.CollectionName + "_create", "icon-ok"));
                    };
                    register(this._subcollections[i]);
                }

                this._actions.push(new CMSViews.NestedListViewAction(this, new CMSViews.CreateActionBehavior(), function () {
                    _this.createItem(_this._collectionName);
                }, "Create", "btn-success", this._collectionName + "_create", subactions));
            } else {
                if (insertPerm)
                    this._actions.push(new CMSViews.ListViewAction(this, new CMSViews.CreateActionBehavior(), function () {
                        _this.createItem(_this._collectionName);
                    }, "Create", "btn-success", this._collectionName + "_create", "icon-plus"));
            }

            if (deletePerm)
                this._actions.push(new CMSViews.ListViewAction(this, new CMSViews.RemoveActionBehavior(), this.removeItems, "Remove", "btn-danger", this._collectionName + "_remove", "icon-remove"));
            if (updatePerm)
                this._actions.push(new CMSViews.ListViewAction(this, new CMSViews.EditActionBehavior(), this.editItem, "Edit", "", this._collectionName + "_edit", "icon-edit"));
            this._actions.push(new CMSViews.ListViewAction(this, new CMSViews.RefreshActionBehavior(), this.refresh, "Refresh", "", this._collectionName + "_refresh", "icon-refresh"));
        };

        ListView.prototype.init = function () {
            this.renderMenu();
            this.onChangeSelection();
            this.renderFilters();

            this.getFilterValues();
        };

        ListView.prototype.renderMenu = function () {
            var container = this._rootElement.find("div#menu");
            if (this._actions.length > 0) {
                container.addClass("form-actions");
                this._actions.forEach(function (p) {
                    return p.render(container);
                });
            }
        };

        ListView.prototype.renderFilters = function () {
            var _this = this;
            var _listView = this;
            var container = this._rootElement.find("div#filter");
            if (this._filters.length > 0) {
                container.addClass("form-inline");
                container.addClass("list-view-filter");
                container.addClass("col-xs-12");


                this._filters.forEach(function (p) {
                    return p.render(_this);
                });
                var buttonId = "FilterApply-" + this._scopeName;
                container.append("<input id=\"" + buttonId + "\" class=\"btn\" type=\"button\" value=\"Применить фильтр\">");

                var scope = this._scopeName;
                var collectionName = this._collectionName;
                var rootElement = this._rootElement;

                var grid = this._grid;

                $("body").delegate("#" + buttonId, "click", function () {
                    var url = '/List/Read/?collectionName=' + collectionName + '&filters=' + encodeURIComponent(_listView.getFilterValues());

                    grid.fnReloadAjax(url);
                    /*self._selectedItems = [];
                    self._selectedAll = false;
                    self.onChangeSelection();*/
                });
            }
        };

        ListView.prototype.getFilterValues = function () {
            var filterParameters = [];
            for (var i = 0; i < this._filters.length; i++) {
                filterParameters.push(this._filters[i]._id);
            }
            var values = '';
            for (var j = 0; j < filterParameters.length; j++) {
                if (this._filters[j] instanceof CMSViews.CheckBoxPropertyFilter) {
                    values += $('#' + filterParameters[j]).is(":checked") + ",";
                    continue;
                }
                //if (!(this._filters[j] instanceof CMSViews.DateRangePropertyFilter)) {
                var filterValue = $('#' + filterParameters[j]).val();
                if(filterValue==undefined)
                    filterValue = "";
                    filterValue.trim().replace(',', '{comma}');

                    values += filterValue + ",";
                //}
            }

            this._actualFilters = values;

            return values;
        };

        ListView.prototype.open = function (id, view) {
            this._createTabMethod(id, view);
        };

        ListView.prototype.getUrl = function () {
            return this._viewMethod + "?collectionNameWithInitFilters=" + this._collectionName + (this._initFilters == null ? '' : '|' + this._initFilters);
        };

        ListView.prototype.selectItem = function (id, state, cmstype) {
            var index = -1;
            for (var i = 0; i < this._selectedItems.length; i++) {
                if (this._selectedItems[i].Id == id) {
                    index = i;
                }
            }
            if (state ? this._selectedAll : !this._selectedAll) {
                if (index != undefined && index >= 0) {
                    this._selectedItems.splice(index, 1);
                }
            } else {
                if (index < 0) {
                    this._selectedItems.push(new SelectedItem(id, cmstype));
                }
            }
            this.onChangeSelection();
        };

        ListView.prototype.selectItems = function (id, state, cmstype) {
            var context = this;
            var index = -1;
            for (var i = 0; i < context._selectedItems.length; i++) {
                if (context._selectedItems[i].Id == id) {
                    index = i;
                }
            }
            //if (state ? context._selectedAll : !context._selectedAll) {
            //    if (index != undefined && index >= 0) {
            //        ListView._selectedItems.splice(index, 1);
            //    }
            //} else {
                if (index < 0) {
                    context._selectedItems.push(new SelectedItem(id, cmstype));
                }
            //}
            this.onChangeSelection();
        };                                                                                                                                                            

        ListView.prototype.selectAllItems = function (state) {
            
            var context = this;
            context._selectedAll = state;
            context._selectedItems = [];
            var table = $('#tablescope3');
            var tr = table.find("tr");
            var checkbox = tr.find("td:first > input[type=checkbox]");
            checkbox.prop("checked", true);
            
            var id = "";
            var cmstype = "";
            var status = true;
            if (checkbox.length > 0) {
               // for (var i = 0; i < checkbox.length; i++) {
                    checkbox.each(function (index, el) {
                      
                        id = $(el).val();
                        context.selectItems(id, status, cmstype/*data.cmstype*/);
                        //    return;
                        //var index = -1;
                        //for (var i = 0; i < ListView.prototype._selectedItems.length; i++) {
                        //    if (ListView.prototype._selectedItems[i].Id == id) {
                        //        index = i;
                        //    }
                        //}
                        //if (index < 0) {
                        //    ListView.prototype._selectedItems.push(ListView.prototype.Id = id);
                        //    ListView.prototype._selectedItems.push(ListView.prototype.Cmstype = cmstype);
                            
                        //}
                        context.onChangeSelection();
                    });

                    //var $asd = checkbox[i];
                    //var status = checkbox.prop("checked");
                    ////if (checkbox[0] != event.target) {
                    ////    status = !status;
                    ////    checkbox.prop('checked', status);
                    ////}
                    //var cmstype = "";
                    //status = true;
                    //this.selectItem(id, status, cmstype/*data.cmstype*/);
                    //    return;
                    //this.onChangeSelection();
                //}
            }
            
        };
        ListView.prototype.getSelectedIds = function () {
            var ids = [];
            for (var i = 0; i < this._selectedItems.length; i++) {
                ids.push(this._selectedItems[i].Id);
            }
            return ids;
        };
        ListView.prototype.createItem = function (collectionName) {
            if (!collectionName) {
                collectionName = this._collectionName;
            }
            var view = new CMSViews.Detail(collectionName, "", new DetailCreateState());
            this._createTabMethod(0, view);
        };

        ListView.prototype.removeItems = function () {
            var view = this;
            deletion.confirm2("Да", "Нет", view.getSelectedIds().length > 1, function () {
                $.ajax({
                    url: "Detail/Remove",
                    datatype: "json",
                    type: "post",
                    data: {
                        collectionName: view._collectionName,
                        items: view.getSelectedIds().join(",")
                    },
                    context: view,
                    async: true,
                    success: function (result) {
                        if (result == "timeout")
                            window.location.href = '/login/?ReturnUrl=%2f';
                        if (result.success) {
                            view.refresh();
                        } else {
                            blackout.alert(result.error, "Ошибка");
                        }
                    },
                    complete: function (result) {
                        blackout.end();

                        var table = $('#tablescope3');
                        var tr = table.find("tr");
                        var checkboxAll = tr.find("th:first > input[type=checkbox]");
                        checkboxAll.prop("checked", false);

                        
                    }
                });
                blackout.start();
            }, undefined);
        };

        ListView.prototype.editItem = function () {
            var id = this._selectedItems[0].Id;
            var view = new CMSViews.Detail(this._collectionName, id + "", new DetailEditState());
            this._createTabMethod(id, view);
        };

        ListView.prototype.refresh = function () {
            this._grid.fnReloadAjax();
            this._selectedItems = [];
            this._selectedAll = false;
            this.onChangeSelection();
        };

        ListView.prototype.onChangeSelection = function () {
            var _this = this;
            this._actions.forEach(function (h) {
                return h.repaint(_this._selectedItems, _this._selectedAll);
            });
        };

        ListView.prototype.assignGrid = function (grid) {
            this._grid = grid;
        };
        return ListView;
    })(CMSViewBase);
    CMSViews.ListView = ListView;

    var TreeView = (function (_super) {
        __extends(TreeView, _super);
        function TreeView(collectionName, createTabMethod, notUsedPar, filters, permissions, initFilters) {
            this._createTabMethod = createTabMethod;
            this._collectionName = collectionName;
            this._selectedAll = false;
            this._selectedItems = [];
            this._actions = [];
            this._filters = filters != null ? filters : [];

            this._viewMethod = 'Tree/Index';
            this._initFilters = initFilters;

            this.fillActions(permissions);

            _super.call(this);
        }
        TreeView.prototype.fillActions = function (permissions) {
            var _this = this;
            var insertPerm = (permissions != null) && (permissions.length > 0) && (permissions[0]);
            var updatePerm = (permissions != null) && (permissions.length > 1) && (permissions[1]);
            var deletePerm = (permissions != null) && (permissions.length > 2) && (permissions[2]);

            if (insertPerm)this._actions.push(new CMSViews.TreeViewAction(this, new CMSViews.CreateActionBehavior(), function () {_this.createItem(_this._collectionName);}, "Создать", "btn-success", this._collectionName + "_create", "icon-plus"));
            if (deletePerm)this._actions.push(new CMSViews.TreeViewAction(this, new CMSViews.RemoveActionBehavior(), this.removeItems, "Удалить", "btn-danger", this._collectionName + "_remove", "icon-remove"));
            if (updatePerm)this._actions.push(new CMSViews.TreeViewAction(this, new CMSViews.EditActionBehavior(), this.editItem, "Правка", "", this._collectionName + "_edit", "icon-edit"));
            this._actions.push(new CMSViews.TreeViewAction(this, new CMSViews.RefreshActionBehavior(), this.refresh, "Обновить", "", this._collectionName + "_refresh", "icon-refresh"));
        };

        TreeView.prototype.init = function () {
            this.renderMenu();
            this.onChangeSelection();
            this.renderFilters();

            this.getFilterValues();
        };

        TreeView.prototype.renderMenu = function () {
            var container = this._rootElement.find("div#menu");
            if (this._actions.length > 0) {
                container.addClass("form-actions");
                this._actions.forEach(function (p) {
                    return p.render(container);
                });
            }
        };

        TreeView.prototype.renderFilters = function () {
            var _this = this;
            var _treeView = this;
            var container = this._rootElement.find("div#filter");
            if (this._filters.length > 0) {
                container.addClass("form-inline");
                container.addClass("list-view-filter");
                container.addClass("col-xs-12");

                this._filters.forEach(function (p) {
                    return p.render(_this);
                });
                var buttonId = "FilterApply-" + this._scopeName;
                container.append("<input id=\"" + buttonId + "\" class=\"btn\" type=\"button\" value=\"Применить фильтр\">");

                var scope = this._scopeName;
                var collectionName = this._collectionName;
                var rootElement = this._rootElement;

                var tree = this._tree;

                $("body").delegate("#" + buttonId, "click", function () {
                    var url = '/Tree/Read/?collectionName=' + collectionName + '&filters=' + _treeView.getFilterValues();
                    window.reloadTree(url);
                    /*self._selectedItems = [];
                    self._selectedAll = false;
                    self.onChangeSelection();*/
                });
            }
        };

        TreeView.prototype.getFilterValues = function () {
            var filterParameters = [];
            for (var i = 0; i < this._filters.length; i++) {
                filterParameters.push(this._filters[i]._id);
            }
            var values = '';
            for (var j = 0; j < filterParameters.length; j++) {
                if (this._filters[j] instanceof CMSViews.CheckBoxPropertyFilter) {
                    values += $('#' + filterParameters[j]).is(":checked") + ",";
                    continue;
                }

                var filterValue = $('#' + filterParameters[j]).val().trim().replace(',', '{comma}');

                values += filterValue + ",";
            }

            this._actualFilters = values;

            return values;
        };

        TreeView.prototype.open = function (id, view) {
            this._createTabMethod(id, view);
        };

        TreeView.prototype.getUrl = function () {
            return this._viewMethod + "?collectionNameWithInitFilters=" + this._collectionName + (this._initFilters == null ? '' : '|' + this._initFilters);
        };

        TreeView.prototype.selectItem = function (selItems) {
            this._selectedItems.splice(0);

            for (var i = 0; i < selItems.length; i++) {
                this._selectedItems.push(selItems[i]);
            }

            this.onChangeSelection();
        };

        TreeView.prototype.selectAllItems = function (state) {
          

            this._selectedAll = state;
            this._selectedItems = [];
            this.onChangeSelection();
        };
        TreeView.prototype.getSelectedIds = function () {
            var ids = [];
            for (var i = 0; i < this._selectedItems.length; i++) {
                ids.push(this._selectedItems[i].Id);
            }
            return ids;
        };
        TreeView.prototype.createItem = function (collectionName) {
            if (!collectionName) {
                collectionName = this._collectionName;
            }
            var view = new CMSViews.Detail(collectionName, "", new DetailCreateState());
            this._createTabMethod(0, view);
        };

        //TreeView.prototype.removeAllItems = function() {
        //    var view = this;
        //    deletion.confirm2("Да", "Нет", view.getSelectedIds().length > 1, function () {
        //        $.ajax({
        //            url: "Detail/Remove",
        //            datatype: "json",
        //            type: "post",
        //            data: {
        //                collectionName: view._collectionName,
        //                items: view.getSelectedIds().join(","),
        //            },
        //            context: view,
        //            async: true,
        //            success: function (result) {
        //                if (result == "timeout") window.location.href = '/login/?ReturnUrl=%2f';
        //                if (result.success) {
        //                    view.refresh();
        //                } else {
        //                    blackout.alert(result.error, "Ошибка");
        //                }
        //            },
        //            complete: function (result) {
        //                blackout.end();
        //            }
        //        });
        //        blackout.start();
        //    }, undefined);
        //}


        TreeView.prototype.removeItems = function () {
            //if (this._selectedAll) return removeAllItems();
            var view = this;
            deletion.confirm2("Да", "Нет", view.getSelectedIds().length > 1, function () {
                $.ajax({
                    url: "Detail/Remove",
                    datatype: "json",
                    type: "post",
                    data: {
                        collectionName: view._collectionName,
                        items: view.getSelectedIds().join(",")
                    },
                    context: view,
                    async: true,
                    success: function (result) {
                        if (result == "timeout")
                            window.location.href = '/login/?ReturnUrl=%2f';
                        if (result.success) {
                            view.refresh();
                        } else {
                            blackout.alert(result.error, "Ошибка");
                        }
                    },
                    complete: function (result) {
                        blackout.end();
                    }
                });
                blackout.start();
            }, undefined);
        };

        TreeView.prototype.editItem = function () {
            var id = this._selectedItems[0].Id;
            var view = new CMSViews.Detail(this._collectionName, id + "", new DetailEditState());
            this._createTabMethod(id, view);
        };

        TreeView.prototype.refresh = function () {
            var url = '/Tree/Read/?collectionName=' + this._collectionName + '&filters=' + this.getFilterValues();
            window.reloadTree(url);
            this._selectedItems = [];
            this._selectedAll = false;
            this.onChangeSelection();
        };

        TreeView.prototype.onChangeSelection = function () {
            var _this = this;
            this._actions.forEach(function (h) {
                return h.repaint(_this._selectedItems, _this._selectedAll);
            });
        };

        TreeView.prototype.assignTree = function (tree) {
            this._tree = tree;
        };
        return TreeView;
    })(CMSViewBase);
    CMSViews.TreeView = TreeView;

    var Root = (function (_super) {
        __extends(Root, _super);
        function Root() {
            _super.apply(this, arguments);
        }
        Root.prototype.setView = function (view) {
            this._childView = view;
            view.assign(this, $("#main-container"));
        };
        return Root;
    })(CMSViewBase);
    CMSViews.Root = Root;

    var Detail = (function (_super) {
        __extends(Detail, _super);
        function Detail(objectType, identifier, state) {
            this._objectType = objectType;
            this._identifier = identifier;
            this._state = state;

            _super.call(this);
        }
        Detail.prototype.changeState = function (state) {
            this._state = state;
            this.setInnerHtml($.ajax({ url: this.getUrl(), context: this, async: false }).responseText);
            this._rootElement.html(this._innerHtml);
        };

        Detail.prototype.save = function () {
            $.bt_validate.result = true;
            this._rootElement.find("form").first().find('input[validate],select[validate],textarea[validate]').trigger('blur');

            //$.bt_validate.form.find('input[validate],select[validate],textarea[validate]').trigger('blur');
            this.submitForm();
        };

        Detail.prototype.submitForm = function (newStateId) {
            if (!$.bt_validate.result) {
                return;
            }

            if ($.bt_validate.blocked) {
                var that = this;
                setTimeout(function () {
                    that.submitForm(newStateId);
                }, 250);
                return;
            }

            tinymce.triggerSave();

            var thisContext = this;
            var form = this._rootElement.find("form").first();

            form.find('.CodeMirror').each(function (i, el) {
                if (el.CodeMirror != undefined)
                    el.CodeMirror.save();
            });

            if (newStateId != null) {
                $('<input>').attr({
                    type: 'hidden',
                    id: 'newStateId',
                    name: 'newStateId',
                    value: newStateId
                }).appendTo(form);
            }

            var isNewObject = this._rootElement.find("#identifier").val() == "";
            var tab = this._tab;

            form.ajaxForm({
                beforeSubmit: function (arr, form, options) {
                    blackout.start();
                    return 0;
                },
                success: function (response, statusText, xhr, $form) {
                    if (response == "timeout")
                        window.location.href = '/login/?ReturnUrl=%2f';
                    if (response.success) {
                        thisContext.setInnerHtml(response.html);
                        thisContext._identifier = response.id + "";
                        thisContext._rootElement.html(thisContext._innerHtml);
                        blackout.end();

                        if (isNewObject) {
                            var oldName = tab._name;
                            var newName = oldName.substring(0, oldName.length - 1) + response.id;
                            tab.changeName(newName);
                        }
                    } else {
                        blackout.end();

                        blackout.alert(response.error, "Ошибка");
                    }
                }
            });
            form.submit();
        };

        Detail.prototype.remove = function () {
            var view = this;
            deletion.confirm2("Да", "Нет", false, function () {
                var collectionName = view._objectType;
                var id = view._identifier;
                var tab = view._tab;

                var detViewGuid = '';
                var detViewInput = view._tab._contentElement.find("#detailViewGuid");
                if (detViewInput != undefined)
                    detViewGuid = detViewInput.val();

                $.ajax({
                    url: "Detail/Remove",
                    datatype: "json",
                    type: "post",
                    data: {
                        collectionName: collectionName,
                        items: id,
                        detailViewGuid: detViewGuid
                    },
                    context: view,
                    async: true,
                    success: function (result) {
                        if (result == "timeout")
                            window.location.href = '/login/?ReturnUrl=%2f';
                        if (result.success) {
                            tab.remove();
                        } else {
                            blackout.alert(result.error, "Ошибка");
                        }
                    },
                    complete: function () {
                        blackout.end();
                    }
                });

                blackout.start();
            }, undefined);
        };

        Detail.prototype.closeTab = function () {
            var detGuidInput = this._rootElement.find("form").first().find("#detailViewGuid");
            if (detGuidInput[0] != undefined) {
                var collectionNameInput = this._rootElement.find("form").first().find("#collectionName");
                $.ajax('/Detail/ClearDetailViewData/?collectionName=' + collectionNameInput.val() + '&detailViewGuid=' + detGuidInput.val());
            }
            this._tab.remove();
        };

        Detail.prototype.getUrl = function () {
            return this._state.getUrl(this._objectType, this._identifier);
        };

        Detail.prototype.open = function (id, view, createTabMethod) {
            createTabMethod(id, view);
        };

        Detail.prototype.changeObjState = function (stateId, preSave) {
            if (preSave) {
                $.bt_validate.result = true;
                this._rootElement.find("form").first().find('input[validate],select[validate],textarea[validate]').trigger('blur');
                this.submitForm(stateId);
            } else {
                blackout.start();
                var view = this;
                var collectionName = view._objectType;
                var id = view._identifier;

                var detViewGuid = '';
                var detViewInput = view._tab._contentElement.find("#detailViewGuid");
                if (detViewInput != undefined)
                    detViewGuid = detViewInput.val();

                $.ajax({
                    url: "Detail/ChangeState",
                    datatype: "json",
                    type: "post",
                    data: {
                        collectionName: collectionName,
                        id: id,
                        stateId: stateId,
                        detailViewGuid: detViewGuid
                    },
                    context: view,
                    async: true,
                    success: function (result) {
                        if (result == "timeout")
                            window.location.href = '/login/?ReturnUrl=%2f';
                        if (result.success) {
                            view.setInnerHtml(result.html);
                            view._rootElement.html(view._innerHtml);
                            blackout.end();
                        } else {
                            blackout.alert(result.error, "Ошибка");
                            blackout.end();
                        }
                    }
                });
            }
        };

        Detail.prototype.sendToYandexWebmaster = function (collectionName, identifier, propertyName, stateId, preSave) {
            tinymce.triggerSave();
            var form = this._rootElement.find("form").first();
            var text = form.find('[name$=' + propertyName + ']').val();
            text = $(text).text().replace(/>/g, '&gt;').replace(/</g, '&lt;').replace(/&/g, '&amp;');

            if (text.length < 500) {
                blackout.alert('Слишком короткий текст (менее 500 символов)');
                return;
            }
            if (text.length > 32000) {
                blackout.alert('Слишком длинный текст (более 32000 символов)');
                return;
            }

            var view = this;
            deletion.confirm('Текст будет добавлен в оригинальные тексты Yandex Webmaster', 'Да', 'Нет', function () {
                $.ajax({
                    url: "Yandex/SendToWebmaster",
                    datatype: "json",
                    type: "post",
                    data: {
                        collectionName: collectionName,
                        id: identifier,
                        text: encodeURIComponent(text)
                    },
                    context: view,
                    async: true,
                    success: function (result) {
                        if (result == "timeout")
                            window.location.href = '/login/?ReturnUrl=%2f';
                        if (result.success) {
                            blackout.alert('Текст добавлен в оригинальные тексты Yandex Webmaster');
                        } else {
                            blackout.alert(result.error, "Ошибка");
                        }
                        if (stateId != undefined)
                            view.changeObjState(stateId, preSave);
                    }
                });
            }, function () {
                if (stateId != undefined)
                    view.changeObjState(stateId, preSave);
            });
        };
        return Detail;
    })(CMSViewBase);
    CMSViews.Detail = Detail;

    var DetailCreateState = (function () {
        function DetailCreateState() {
        }
        DetailCreateState.prototype.getUrl = function (objectType, identifier) {
            return "Detail/Edit?collectionName=" + objectType;
        };
        return DetailCreateState;
    })();
    CMSViews.DetailCreateState = DetailCreateState;
    var DetailReadState = (function () {
        function DetailReadState() {
        }
        DetailReadState.prototype.getUrl = function (objectType, identifier) {
            return "Detail/Read?collectionName=" + objectType + "&identifier=" + identifier;
        };
        return DetailReadState;
    })();
    CMSViews.DetailReadState = DetailReadState;
    var DetailEditState = (function () {
        function DetailEditState() {
        }
        DetailEditState.prototype.getUrl = function (objectType, identifier) {
            return "Detail/Edit?collectionName=" + objectType + "&identifier=" + identifier;
        };
        return DetailEditState;
    })();
    CMSViews.DetailEditState = DetailEditState;

    var StubView = (function (_super) {
        __extends(StubView, _super);
        function StubView() {
            _super.apply(this, arguments);
        }
        return StubView;
    })(CMSViewBase);
    CMSViews.StubView = StubView;

    var LinkToMany = (function (_super) {
        __extends(LinkToMany, _super);
        function LinkToMany(collection, identifier, selector) {
            this._collection = collection;
            this._identifier = identifier;
            _super.call(this);
            this.assign(null, $(selector));
        }
        LinkToMany.prototype.getUrl = function () {
            return "Link/ToManyIndex?collectionName=" + this._collection + "&identifier=" + this._identifier;
        };
        return LinkToMany;
    })(CMSViewBase);
    CMSViews.LinkToMany = LinkToMany;

    var LinkToOne = (function (_super) {
        __extends(LinkToOne, _super);
        function LinkToOne(collection, identifier, selector) {
            this._collection = collection;
            this._identifier = identifier;
            _super.call(this);
            this.assign(null, $(selector));
        }
        LinkToOne.prototype.getUrl = function () {
            return "Link/LinkToOne?collectionName=" + this._collection + "&identifier=" + this._identifier;
        };
        return LinkToOne;
    })(CMSViewBase);
    CMSViews.LinkToOne = LinkToOne;

    var DashboardView = (function (_super) {
        __extends(DashboardView, _super);
        function DashboardView() {
            _super.apply(this, arguments);
        }
        DashboardView.prototype.getUrl = function () {
            return "Dashboard";
        };
        return DashboardView;
    })(CMSViewBase);
    CMSViews.DashboardView = DashboardView;

    var HistoryKeeper = (function () {
        function HistoryKeeper() {
            this._list = [];
            this._position = -1;
            this._increment = 0;
            if (HistoryKeeper._instance) {
                throw new Error("Error: Instantiation failed: Use getInstance() instead of new.");
            }
            HistoryKeeper._instance = this;
        }
        HistoryKeeper.getInstance = function () {
            if (HistoryKeeper._instance === null) {
                HistoryKeeper._instance = new HistoryKeeper();
            }
            return HistoryKeeper._instance;
        };

        HistoryKeeper.prototype.push = function (tab) {
            this._position++;
            this._list.slice(this._position);
            this._list.push(tab);
            history.pushState({ "name": tab._name, "index": this._increment++ }, tab._name, null);
        };

        HistoryKeeper.prototype.move = function (tabName, isBack) {
            var result = $.grep(this._list, function (e) {
                return e._name == tabName;
            });

            if (result.length > 0) {
                var tab = result[0];
                tab.activate();
                this._position = this._list.indexOf(tab);
            } else {
                if (isBack) {
                    history.back();
                } else {
                    history.forward();
                }
            }
        };

        HistoryKeeper.prototype.back = function () {
            if (this._position >= 0) {
                this._position--;
            }
            if (this._position >= 0) {
                var tab = this._list[this._position];
                tab.activate();
            }
        };

        HistoryKeeper.prototype.forward = function () {
            if (this._position < this._list.length - 1) {
                this._position++;
                var tab = this._list[this._position];
                tab.activate();
            }
        };

        HistoryKeeper.prototype.removeFromHistory = function (tab) {
            var index = -1;
            while ((index = this._list.indexOf(tab)) >= 0) {
                this._list.splice(index, 1);
            }
        };
        HistoryKeeper._instance = null;
        return HistoryKeeper;
    })();
    CMSViews.HistoryKeeper = HistoryKeeper;
})(CMSViews || (CMSViews = {}));
//# sourceMappingURL=Views.js.map
