
var oTree__CMSScopeName_ = undefined;

$(document).ready(function () {
    reloadTree('@Html.Raw(Model.Url)');
});


function reloadTree(url) {

    $('#pager_CMSScopeName_').next().remove();
    $('#pager_CMSScopeName_').after('<div id="tree_CMSScopeName_" class="tree"></div>');

    var treeDataSource = new DataSourceTree({
        url: url,
        collectionName: '@(Model.Type.Name)'
    });
    
    var treeDiv = $('#tree_CMSScopeName_');
    treeDiv.html('<div class = "tree-folder" style="display:none;">\
				<div class="tree-folder-header">\
					<i class="ace-icon tree-plus"></i>\
					<div class="tree-item-check">' +
        //'<i class="ace-icon ace-icon fa fa-times"></i>' +
        '</div>\
                    <div class="tree-folder-name"></div>\
				</div>\
				<div class="tree-folder-content"></div>\
				<div class="tree-loader" style="display:none"></div>\
			</div>\
			<div class="tree-item" style="display:none;">\
                <div class="tree-item-check">' +
        //'<i class="ace-icon ace-icon fa fa-times"></i>' +
        '</div>\
                <div class="tree-item-name"></div>\
            </div>');
   
    treeDiv.addClass('tree-selectable');

    oTree__CMSScopeName_ = treeDiv.tree({
        dataSource: treeDataSource,
        multiSelect: true,
        loadingHTML: '<div class="tree-loading"><i class="ace-icon fa fa-refresh fa-spin blue"></i></div>',
        'open-icon': 'ace-icon tree-minus',
        'close-icon': 'ace-icon tree-plus',
        'selectable': true,
        'selected-icon': 'ace-icon fa fa-check',
        'unselected-icon': 'ace-icon fa fa-times'

    });


   

    oTree__CMSScopeName_.on('selected', function (evt, data) {

        var selItems = [];
        for (var i = 0; i < data.info.length; i++) {
            selItems.push(new CMSViews.SelectedItem(data.info[i].id, ''));
        }

        _CMSScope_.selectItem(selItems);
        tab.open('@(cmsMetadataAttribute.CollectionTitleSingular) #', '@(Model.Type.Name)', data.id);
    });

    oTree__CMSScopeName_.on('click', function (evt, data) {

        tab.open('@(cmsMetadataAttribute.CollectionTitleSingular) #', '@(Model.Type.Name)', data.id);

    });

    _CMSScope_.assignTree(oTree__CMSScopeName_);
}