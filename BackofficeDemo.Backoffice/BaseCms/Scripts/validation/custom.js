$(function () {
    $.bt_validate.method(
        "regexp",
        function (value, expression) {
            expression = expression.replace(new RegExp("{{quot}}", 'g'), "\"").replace(new RegExp("{{stick}}", 'g'), "|").replace(new RegExp("{{comma}}", 'g'), ",");
            var re = new RegExp(expression);
            return re.test(value) || value.length == 0;
        },
        "Неверное значение поля!");
    
    $.bt_validate.method(
        'unique',
        $.bt_validate.ajax_check({
            url: '/Validation/Unique',
            type: 'POST',
            return_type: 'text',
            get_data: function (identifier, value, expression) {
                var params = expression.split(';');
                var collectionName = params[0];
                var propertyNames = params[1];

                var currentScope = getActiveScopeName();
                var values = '';

                var propNamesArray = propertyNames.split('+');
                for (var i = 0; i < propNamesArray.length; i++) {
                    values += $('#' + currentScope + propNamesArray[i]).val() + ((i < propNamesArray.length - 1) ? '+' : '');
                }
                
                return {
                    'collectionName': collectionName,
                    'propertyName': propertyNames,
                    'value': values,
                    'identifier': identifier,
                };
            },
            get_success: function (res) {
                return (res != "True");
            },
            msg_ok: 'Значение уникально',
            msg_checking: 'Проверка уникальности ...',
            msg_fail: 'Значение поля должно быть уникальным!'
        })
    );
    
    $.bt_validate.method(
        "equal",
        function (value, expression) {
            var scope = getActiveScopeName();
            return $('#' + scope + expression).val() == value;
        });
    
    $.bt_validate.method(
        "jscriptfunc",
        function (value, expression) {
            return window[expression](value);
        });
});