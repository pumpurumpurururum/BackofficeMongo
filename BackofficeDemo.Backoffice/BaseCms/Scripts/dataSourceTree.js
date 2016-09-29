var DataSourceTree = function (options) {
    this.url = options.url;
    this.collectionName = options.collectionName;
};

DataSourceTree.prototype.data = function (options, callback) {
    var param = null;
    if (!("name" in options) && !("type" in options)) {
        param = 0;//load the first level data
    }
    else if ("type" in options && options.type == "folder") {
        if ("id" in options)
            param = options.id;
    }

    if (param != null) {
        $.ajax({
            url: this.url,
            data: 'collectionName=' + this.collectionName + '&itemId=' + param,
            type: 'POST',
            dataType: 'json',
            success: function (response) {

                if (response.status == "OK")
                    callback({ data: response.data });
            },
            error: function (response) {
                //console.log(response);
            }
        });
    }
};

