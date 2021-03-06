﻿/**
 * Bootstrap validate V 0.1 Alpha
 * 
 * No license
 * Do what you want with it
 */
$.bt_validate = {
  method: function(method_name, fn_check, message) {
    $.bt_validate.fn[method_name] = fn_check;
    $.bt_validate.text[method_name] = message;
  }
}

$.fn.show_tooltip = function (text, color) {
    
    // Если ошибка произошла на другой вкладке, ищем эту вкладку и открываем ее.
    var form = $(this).parents("form");
    if (form.length > 0) {
        var tabs = $(form).find(".tab-pane");
        if (tabs.length > 0) {
            //var t = $(tabs[0]);
            var t = $(this).parents(".tab-pane").first();
            if (!t.hasClass("active")) {
                $(form).find(".tabbable").find("li").removeClass("active");
                $(form).find(".tab-pane").removeClass("active");
                t.addClass("active");
                var id = $(t).attr("id");
                $(form).find('a[href="#' + id + '"]').parent().addClass("active");
            }
        }
    }
    
    var el = $(this).data("select2") != undefined ?
        $(this).parent().find(".select2-container")[0] :
        $(this).hasClass("num") ?
            $(this).parent() :
        $(this);
    
    $(el).tooltip('destroy');
    
    var rid = 'tlt_' + parseInt(new Date().getTime());
    var marker = '<div id="'+rid+'"</div>';
    $(el).tooltip(
      { title: text + marker, html: true, trigger: 'manual', placement: 'right' });
    
    $(el).tooltip('show');
    $('#'+rid).parent().css({'background-color': color});
    $('#'+rid).parent().prev().css({'border-right-color': color});
}
  
$.fn.show_ok_tooltip = function(text) {
    $(this).show_tooltip(text, 'green');
}

$.fn.show_err_tooltip = function(text) {
    $(this).show_tooltip(text, 'red');
}

$.fn.hide_tooltip = function (text) {
    var el = $(this).data("select2") != undefined ?
        $(this).parent().find(".select2-container")[0] :
        $(this).hasClass("num") ?
            $(this).parent() :
        $(this);

    $(el).tooltip('destroy');
}


$.bt_validate.result = true;
$.bt_validate.blocked = false;

$.bt_validate.block = function() {
  $.bt_validate.blocked = true;
}

$.bt_validate.unblock = function() {
  $.bt_validate.blocked = false;
}

$.fn.bt_validate = function() {
  $.bt_validate.form = $(this);
  $.bt_validate.form.find('input[validate],select[validate],textarea[validate]').blur(validateField);
  
  $.bt_validate.form.submit(function() {
    /*$.bt_validate.result = true;
    $.bt_validate.form.find('input[validate],select[validate],textarea[validate]').trigger('blur');
    
    if($.bt_validate.blocked) return false;*/
    return $.bt_validate.result;
  });
}

$.bt_validate.validate = function(params) {
    $.bt_validate.result = true;
    $.bt_validate.form.find('input[validate],select[validate],textarea[validate]').trigger('blur');
    
    if($.bt_validate.blocked) return false;
    return $.bt_validate.result;
}

$.bt_validate.ajax_check = function(params) {
  
  this.params = params;
  var ajax_check = this;
  
  this.fn_validate = function(value, expression) {
    //Checking if value has changed
    if((ajax_check.usercheck_prev_value != undefined) && (ajax_check.usercheck_prev_value == value))
      return '';


    var identifier = $(this).parents("form").find("input[name=identifier]").val();

    ajax_check.usercheck_prev_value = value;
    //Blocking form and showing progress
      $.bt_validate.block();
    this.show_tooltip(ajax_check.params.msg_checking, 'blue');

    var self = this;
    //Timeout
    setTimeout(function() {
        //Ajax call
        $.ajax({
          url: ajax_check.params.url, 
          data: ajax_check.params.get_data(identifier, value, expression),
          success: function(res) {
            if(ajax_check.params.get_success(res)) {
              //User with same email is found
              self.show_err_tooltip(ajax_check.params.msg_fail);
            }
            else {
              //If no user found, showing "green" tooltip and unblocking the form
              self.show_ok_tooltip(ajax_check.params.msg_ok);
              $.bt_validate.unblock();
              //Hiding tooltip after 3 sec
              setTimeout(function() { self.hide_tooltip(); }, 3000);
            }
          }, 
          type: ajax_check.params.type,
          dataType: ajax_check.params.return_type});      
    },250);
    //Return empty string, if you want to skip standart checking
    return '';
  }
  return this;
}

function validateField() {
    if ($(this).attr('validate') == "") {
        return;
    }

    var validate_params = $(this).attr('validate').split('|');

    var field_result = true;

    for (var i = 0; i < validate_params.length; i++) {
        var validate_param = validate_params[i].split(',');
        var fn_name = validate_param[0];

        var match = fn_name.match("\\(.+\\)");
        var errorMessage = null;
        if (match != null) {
            errorMessage = match[0];
            fn_name = fn_name.substr(0, fn_name.indexOf(errorMessage));
            errorMessage = errorMessage.replace("(", "").replace(")", "");
        }

        validate_param[0] = $(this).val().trim();

        var fn_or_object = $.bt_validate.fn[fn_name];

        if (typeof (fn_or_object) == 'function')
            //Validate by function
            var res = fn_or_object.apply($(this), validate_param);
            //var res = fn_or_object.call($(this), validate_param);
        else
            //Validate by object
            var res = fn_or_object.fn_validate.apply($(this), validate_param);
            //var res = fn_or_object.fn_validate.call($(this), validate_param);

        if (typeof (res) != 'string') {
            if (!res) {
                var tl_text = errorMessage;
                if (tl_text == null) {
                    tl_text = $.bt_validate.text[fn_name];
                }

                for (var j = 1; j < validate_param.length; j++) {
                    tl_text = tl_text.replace('%' + j, validate_param[j]);
                }

                $(this).show_err_tooltip(tl_text);

                field_result = false;
                $.bt_validate.result = false;
                //After validate event
                if ($.bt_validate.after_validate != null)
                    $.bt_validate.after_validate.call($(this), fn_name, validate_param[0], validate_param.slice(1), false);
                break;
            }
        }
        else {
            field_result = false;
            //After validate event
            if ($.bt_validate.after_validate != null)
                $.bt_validate.after_validate.call($(this), fn_name, validate_param[0], validate_param.slice(1), false);
        }

    }

    //If result is true
    if (field_result) {
        //After validate event
        if ($.bt_validate.after_validate != null)
            $.bt_validate.after_validate.call($(this), fn_name, validate_param[0], validate_param.slice(1), true);
        //Hiding tooltip
        $(this).tooltip('hide');
    }
}

function validateInputs(inputJqueryCollection) {
    inputJqueryCollection.each(validateField);
}