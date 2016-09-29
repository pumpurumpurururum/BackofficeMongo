if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (what, i) {
        i = i || 0;
        var L = this.length;
        while (i < L) {
            if (this[i] === what) return i;
            ++i;
        }
        return -1;
    };
}

Array.prototype.remove = function () {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};

tinyMceHelper = {
    init: function (id, htmlEditorSettings) {

        var css = '/BaseCms/Content/themes/ace/assets/css/font-awesome.min.css';
        var cssForRemoveFormat = '';
        var needRemoveHtmlTagsButton = false;
        var imagesUrl = undefined;

        if (htmlEditorSettings != undefined) {
            if (htmlEditorSettings.css != undefined) css = css + ',' + htmlEditorSettings.css;
            if (htmlEditorSettings.cssForRemoveFormat != undefined) cssForRemoveFormat = htmlEditorSettings.cssForRemoveFormat;
            if (htmlEditorSettings.needRemoveHtmlTagsButton != undefined) needRemoveHtmlTagsButton = htmlEditorSettings.needRemoveHtmlTagsButton;
            if (htmlEditorSettings.imagesUrl != undefined) imagesUrl = htmlEditorSettings.imagesUrl;
        }

        var wordMode = false;

        tinymce.init({
            selector: id,
            height: 300,
            plugins: 'anchor contextmenu fullscreen heading image link lists media paste preview table textcolor moxiemanager code codemirror',
            language: 'ru',
            skin: 'lightgray',
            menubar: false,
            //menu: {
            //    view: { title: 'View', items: 'visualaid | fullscreen preview' },
            //    format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats' },
            //    table: { title: 'Table', items: 'inserttable  tableprops deletetable | cell row column' },
            //    tools: { title: 'Tools', items: 'removeformat | code' }
            //},
            content_css: css,
            contextmenu: "link image inserttable | cell row column deletetable | code fullscreen",
            // moxie manager props
            imagesUrl: imagesUrl,
            //
            paste_word_valid_elements: "b,strong,i,em,h1,h2,h3,h4,u,sup,sub,span,p,table,tr,td,tbody,th,a[href,target]",
            paste_data_images: true,
            toolbar: 'undo redo | bold italic strikethrough subscript superscript small | h1 h2 h3 h4 blockquote | alignleft aligncenter alignright alignjustify | table | bullist numlist outdent indent | link unlink | image photogallery | removeformat removeHtmlTags triads | code preview fullscreen',
            formats: {
                small: { inline: 'small' },
                removeformat: [
					{
					    selector: 'em,i,font,u,strike,sub,sup,' + cssForRemoveFormat,
					    remove: 'all',
					    split: true,
					    expand: false,
					    block_expand: true,
					    deep: true
					},
					{ selector: 'span', remove: 'empty', split: true, expand: false, deep: true },
					{ selector: '*', attributes: ['class', 'style', 'lang', 'nowrap', 'width', 'align', 'name'], split: false, expand: false, deep: true }
                ]
            },
            codemirror: {
                indentOnInit: true,
                path: 'CodeMirror',
            },
            setup: function (ed) {

                ed.addButton('photogallery', {
                    title: 'Вставить фотогалерею',
                    image: '/BaseCms/Scripts/tinymce/skins/lightgray/img/object.gif',
                    onclick: function () {
                        ed.insertContent('<span class="icon-film">&nbsp;</span>');
                    }
                });
                
                ed.addButton('small', {
                    icon: 'tag',
                    tooltip: '\u0423\u043c\u0435\u043d\u044c\u0448\u0435\u043d\u0438\u0435 \u0440\u0430\u0437\u043c\u0435\u0440\u0430 \u0448\u0440\u0438\u0444\u0442\u0430',
                    onclick: function () {
                        if ((ed.selection.getNode().outerHTML.indexOf('<small>') > -1) && (ed.selection.getNode().outerHTML.indexOf('</small>') > -1)) {
                            var element = ed.selection.getNode();
                            element.outerHTML = element.outerHTML.replace(/<small>/g, '').replace(/<\/small>/g, '');
                        } else {
                            ed.formatter.apply('small');
                        }
                    }
                });

                if (needRemoveHtmlTagsButton) {
                    ed.addButton('removeHtmlTags', {
                        title: 'Очистить HTML форматирование',
                        icon: 'code',
                        text: 'X',
                        onclick: function() {
                            var content = tinymce.activeEditor.selection.getContent();
                            var text = $(content).text();
                            if (text != '') {
                                tinymce.activeEditor.selection.setContent(text);
                                tinymce.activeEditor.undoManager.add();
                            }
                        }
                    });
                }

                var copyActionButtonsToTinyMceEditorStatusBar = function () {
                    
                    // Создаём div для кнопок в статус баре
                    $('div.form-actions[id != menu]').first().after('<div id="form-actions-status-bar" class="form-actions-status-bar"></div>');
                    
                    // Копируем кнопки
                    $('#form-actions-status-bar').append($('div.form-actions[id != menu]').find('button').clone());
                    
                    // Задаём id, стили, обработчики нажатия для кнопок в статус баре
                    var newButtons = $('#form-actions-status-bar').find('button');

                    for (var i = 0; i < newButtons.length; i++) {
                        var button = $(newButtons[i]);
                        button.attr('id', button.attr('id') + '_sbar');
                        button.addClass('btn-minier form-actions-status-bar-button');
                    }

                    newButtons.click(function () {
                        tinyMCE.activeEditor.execCommand('mceFullScreen');
                        $('#' + this.id.replace('_sbar', '')).click();
                    });
                };

                var removeFormActionsStatusBar = function () {
                    $('#form-actions-status-bar').remove();
                };


                ed.on('FullscreenStateChanged', function(e) {
                    // Если полноэкранный режим включён, убираем заголовки активных табов
                    if (e.state) {
                        $('li.active').hide();
                        copyActionButtonsToTinyMceEditorStatusBar();
                    } else {
                        // иначе восстанавливаем заголовки
                        $('li.active').show();
                        removeFormActionsStatusBar();
                    }
                });

                ed.on('PastePreProcess', function (e) {
                    if (e.content.indexOf('<w:WordDocument>') > -1) {
                        wordMode = true;
                        e.content = e.content.replace('–', '&mdash;');
                    }
                });
                
                ed.on('PastePostProcess', function (e) {
                    if (wordMode) {
                        setTimeout(function() {
                            if (confirm("Очистить HTML вёрстку?")) {
                                var editor = tinymce.activeEditor;
                                editor.selection.select(editor.getBody(), true);
                                editor.execCommand('removeFormat');
                            }
                        }, 500);
                    }
                });

                ed.addButton('triads', {
                    title: 'Разбить числа на триады',
                    //text: '0 000',
                    icon: 'triads',
                    onclick: function() {
                        var content = tinymce.activeEditor.selection.getContent();

                        if (content != '') {
                            content = triads(content);
                            tinymce.activeEditor.selection.setContent(content);
                        }
                    }
                });
            }
        });
    },

    initReadOnly: function(id) {
        tinymce.init({
            selector: id,
            height: 300,
            mode: "textareas",
            readonly: true,
            menubar: false,
            toolbar: "false",
        });
    }
};

//function trimSaveContent(elementId, html, body) {
//    html = html.replace(/<!--.*?-->/g, '');
//    return html;
//}

//function convertUrl(url, node, onSave) {
//    if (node == 'a') return url;
//    if (tinymce.activeEditor.settings.imagesUrl != undefined) {
//        var convertUrl = tinymce.activeEditor.settings.imagesUrl;
//        if (convertUrl[convertUrl.length - 1] != '/') convertUrl += '/';
//        return convertUrl + url.substring(url.lastIndexOf('/') + 1);
//    }
//    return url;
//}

blackout = {
    start: function() {
        $("#loading-modal").modal({ backdrop: 'static' });
        
    },
    end: function() {
        $("#loading-modal").modal("hide");
        $(".modal-backdrop").remove();
    },

    alert: function (text, title) {


        bootbox.dialog({
            message: text,

            buttons: {
                success: {
                    label: "Да",
                    className: "btn-success",
                    callback: function () {
                        //if (yesCallback != undefined) yesCallback.call();
                    }
                },

                main: {
                    label: "Нет",
                    className: "btn-primary",
                    callback: function () {
                        //if (noCallback != undefined) noCallback.call();
                    }
                }
            }
        });



        //var modal = $("<div class='modal  fade' id='custom-alert'>" +
        //    (title != undefined ? "<div class ='modal-header'><h4 class='blue bigger'>" + title + "</h4></div>" : "") +
        //    "<div class='modal-body'>" + text + "</div>" +
        //    "<div class ='modal-footer'>" +
        //    "<button id='custom-alert-ok' class='btn btn-small btn-primary'><i class='icon-ok'></i>Ок</button>" +
        //    "</div>" +
        //    "</div>");
        //modal.find("#custom-alert-ok").click(function() {
        //    modal.modal("hide");
        //});
        //modal.on('hidden', function() { modal.remove(); });
        //$("body").append(modal);

        //modal.modal("show");
    },
};

transliteration = {
    chOrigTable: ['а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
        'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т',
        'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь',
        'э', 'ю', 'я', ' '],

    chSafeTable: ['a', 'b', 'v', 'g', 'd', 'e', 'e', 'j', 'z', 'i',
        'i', 'k', 'l', 'm', 'n', 'o', 'p', 'r', 's', 't',
        'y', 'f', 'h', 'c', 'c', 's', 's', '_', 'i', '_',
        'e', 'u', 'a', '-'],

    latins: ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
    ],

    digits: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'],

    transform: function(cyrillicText) {

        var resultText = '';

        cyrillicText = cyrillicText.trim();

        if (cyrillicText != '') {
            cyrillicText = cyrillicText.toLowerCase();

            for (var i = 0; i < cyrillicText.length; i++) {
                if (this.digits.indexOf(cyrillicText[i]) > -1) {
                    resultText += cyrillicText[i];
                    continue;
                }
                
                if (this.latins.indexOf(cyrillicText[i]) > -1) {
                    resultText += cyrillicText[i];
                    continue;
                }

                var index = this.chOrigTable.indexOf(cyrillicText[i]);

                if (index > -1) {
                    resultText += this.chSafeTable[index];
                } else if ((cyrillicText[i] != '.') && (cyrillicText[i] != '-')) {
                    resultText += '-';
                }
            }
        }

        return resultText;
    }
};

function getActiveScopeName() {
    var activeListItems = $('li.active');
    for (var i = 0; i < activeListItems.length; i++) {
        var listItemName = activeListItems[i].id;
        if (listItemName.substring(0, 5) == 'scope') return listItemName.replace('_tab', '');
    }
    return null;
}

function getElementScopeName(elementId) {
    var currectTabId = $('#' + elementId).parent('.tab-pane').attr('id');

    return $('a[href="#' + currectTabId + '"]').parent().attr('id').replace('_tab', '');
}

function checkPasswordIsNecessary(value) {
    if (value != '') return true;

    var currentActiveTab = $('.tab-pane.active');

    var curUserId = currentActiveTab.find('input[name=identifier]').val();
    if (curUserId == '') return false;
    
    return !currentActiveTab.find('input[name=ChangePassword]').is(':checked');
}

var history_previous_tab_index = 9007199254740992;
window.onload = function() {
    if (typeof history.pushState === "function") {
        window.onpopstate = function(event) {
            if (event.state != undefined) {
                var tabName = JSON.stringify(event.state.name).replace(/\"/g, "");

                var isBack = true;
                if (event.state.index > history_previous_tab_index) isBack = false;

                CMSViews.HistoryKeeper.getInstance().move(tabName, isBack);

                history_previous_tab_index = event.state.index;
            }
        };
    }
};

deletion = {
    confirm: function (questionText, yesAnswer, noAnswer, yesCallback, noCallback) {
        /*bootbox.dialog(questionText, [{
            "label": yesAnswer,
            "class": "btn-primary",
            "callback": function () {
                if (yesCallback != undefined) yesCallback.call();
            }
        },
                {
                    "label": noAnswer,
                    "class": "null",
                    "callback": function () {
                        if (noCallback != undefined) noCallback.call();
                    }
                }]
        );*/
		bootbox.dialog({
  message: questionText,
  
  buttons: {
    success: {
      label: yesAnswer,
      className: "btn-success",
      callback: function() {
        if (yesCallback != undefined) yesCallback.call();
      }
    },
    
    main: {
      label: noAnswer,
      className: "btn-primary",
      callback: function() {
        if (noCallback != undefined) noCallback.call();
      }
    }
  }
});
		
		
		
    },
    
    confirm2: function (yesAnswer, noAnswer, multiple, yesCallback, noCallback) {
        $("#dialog-confirm").dialog({
            resizable: false,
            modal: true,
            buttons: [
                {
                    html: "<i class='fa fa-chevron-down bigger-110'></i>&nbsp; " + yesAnswer,
                    "class": "btn btn-danger btn-mini",
                    click: function () {
                        if (yesCallback != undefined) yesCallback.call();
                        $(this).dialog("close");
                    }
                },
                {
                    html: "<i class='ace-icon fa fa-times bigger-110'></i>&nbsp; " + noAnswer,
                    "class": "btn btn-mini",
                    click: function () {
                        if (noCallback != undefined) noCallback.call();
                        $(this).dialog("close");
                    }
                }
            ]
        });

        var val = multiple ? 'записей' : 'записи';
        if (multiple) {
            $("#dialog-confirm div.alert").html('Записи будут удалены без возможности восстановления');
            $("#dialog-confirm").removeClass("hide");
            //$("#dialog-confirm").addClass("block-x");
            
            
        } else {
            $("#dialog-confirm div.alert").html('Запись будет удалена без возможности восстановления');
            $("#dialog-confirm").removeClass("hide");
            //$("#dialog-confirm").addClass("block-x");
           

           
        }

        $(".ui-dialog-title").html("<div class='widget-header'><h4 class='smaller'><i class='fa fa-info-circle red'></i> Удаление " + val + "</h4></div></div>");
    }
};

browserDetection = {
    isIE: function() {
        return navigator.appName.indexOf("Internet Explorer") != -1;
    },
    
    isMac: function() {
        return navigator.userAgent.indexOf('Mac') != -1;
    }
};

tab = {
    open: function(title, collectionName, id) {
        CMSViews.Helper.Get('scope1').createOrActivateTab(title + '' + id, new CMSViews.Detail(collectionName, id, new CMSViews.DetailEditState()));
    }
};

function setMenuStyles(menuItemElement) {
    var menuItem = $(menuItemElement);
    $('div#sidebar li').removeClass('active');
    
    menuItem.parents('li').addClass('active');
}

function triads(text){
    var pattern = /[\s?!>()-+*=#№$]\d{4,}/g;

    var matches = text.match(pattern);
    for (var m = 0; m < matches.length; m++) {
        var replaceText = matches[m];
        var result = '';
        for (var i = replaceText.length - 1; i >= 0; i--) {
            var symbol = replaceText[i];
            result += symbol;
            if (!(symbol >= '0' && symbol <= '9')) break;
            if ((replaceText.length - i) % 3 == 0) result += ' ';
        }

        var reverse = result.split("").reverse().join("");

        text = text.replace(replaceText, reverse);
    }

    return text;
}
