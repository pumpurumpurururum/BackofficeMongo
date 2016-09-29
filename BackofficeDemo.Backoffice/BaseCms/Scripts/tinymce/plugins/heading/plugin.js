tinymce.PluginManager.add('heading', function (editor) {
    
    for (var i = 1; i <= 6; i++) {

        editor.addCommand('mceHeading' + i, function () {
            var ct = editor.getParam("heading_clear_tag", false) ? ed.getParam("heading_clear_tag", "") : "";
            if (editor.selection.getNode().nodeName.toLowerCase() != 'h' + this) {
                ct = 'h' + this;
                var nodes = editor.selection.getNode().childNodes;
                var tagsToRemove = ['strong', 'b', 'i', 'em'];
                for (var j = 0; j < nodes.length; j++) {
                    if (nodes[j].outerHTML == undefined) continue;
                    for (var k = 0; k < tagsToRemove.length; k++) {
                        if (nodes[j].outerHTML == undefined) break;
                        nodes[j].outerHTML = nodes[j].outerHTML.replace('<' + tagsToRemove[k] + '>', '').replace('</' + tagsToRemove[k] + '>', '');
                    }
                }
            }
            editor.execCommand('FormatBlock', false, ct);
        }, i);
        
        editor.addButton('h' + i, {
            text: 'H' + i,
            tooltip: '\u0417\u0430\u0433\u043e\u043b\u043e\u0432\u043e\u043a ' + i,
            cmd: 'mceHeading' + i
        });
    }
    
});


