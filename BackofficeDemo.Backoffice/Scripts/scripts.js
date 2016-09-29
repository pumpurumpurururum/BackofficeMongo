﻿var my = {
    removeImagesBySize: function() {
        var rmModal = $("<div id=\"onpage-help-modal\" class=\"modal onpage-help-modal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"HelpModalDialog\" aria-hidden=\"true\">\
			  <div class=\"modal-dialog\">\
				<div class=\"modal-content\">\
					<div class=\"modal-header\">\
					  <div class=\"pull-right onpage-help-modal-buttons\">\
						<button aria-hidden=\"true\" data-dismiss=\"modal\" class=\"btn btn-white btn-danger btn-sm\" type=\"button\"><i class=\"ace-icon fa fa-times icon-only\"></i></button>\
					  </div>\
					  <h4 class=\"modal-title\">Remove all autogenerated images</h4>\
					</div>\
					<div class=\"modal-body\"><div class=\"onpage-help-content\"></div></div>\
                    <div class=\"modal-footer\"><a href=\"#\" class=\"btn btn-success removeimages\">Remove</a> </div>\
				</div>\
			  </div>\
			</div>").appendTo("body");

        var mbody = rmModal.find(".modal-body");
        var btn = rmModal.find(".removeimages");

        btn.on("click", function () {

            $.post("/tools/RemoveImages",
            function (data) {
                if (data.Success) {
                    alert(data.count);
                } else {
                    
                }
                rmModal.modal("hide");
            });

            
        });
        mbody.css({
            'overflow-y': "auto",
            'overflow-x': "hidden"
        });

        rmModal.css({ 'overflow': "hidden" })
            .on("show.bs.modal", function() {
                
            })
            .on("hidden.bs.modal", function() {
                rmModal.remove();
            });
        rmModal.modal("show");
    },
    rebuildIndexes: function () {
        var rmModal = $("<div id=\"onpage-help-modal\" class=\"modal onpage-help-modal\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"HelpModalDialog\" aria-hidden=\"true\">\
			  <div class=\"modal-dialog\">\
				<div class=\"modal-content\">\
					<div class=\"modal-header\">\
					  <div class=\"pull-right onpage-help-modal-buttons\">\
						<button aria-hidden=\"true\" data-dismiss=\"modal\" class=\"btn btn-white btn-danger btn-sm\" type=\"button\"><i class=\"ace-icon fa fa-times icon-only\"></i></button>\
					  </div>\
					  <h4 class=\"modal-title\">Rebuild Indexes</h4>\
					</div>\
					<div class=\"modal-body\"><div class=\"onpage-help-content\"></div></div>\
                    <div class=\"modal-footer\"><a href=\"#\" class=\"btn btn-success removeimages\">Rebuild</a> </div>\
				</div>\
			  </div>\
			</div>").appendTo("body");

        var mbody = rmModal.find(".modal-body");
        var btn = rmModal.find(".removeimages");

        btn.on("click", function () {

            $.post("/tools/RebuildIndexes",
            function (data) {
                if (data.Success) {
                    alert(data.count);
                } else {

                }
                rmModal.modal("hide");
            });


        });
        mbody.css({
            'overflow-y': "auto",
            'overflow-x': "hidden"
        });

        rmModal.css({ 'overflow': "hidden" })
            .on("show.bs.modal", function () {

            })
            .on("hidden.bs.modal", function () {
                rmModal.remove();
            });
        rmModal.modal("show");
    }
};