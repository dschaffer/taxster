var validateForm = function () {
    if (!validation.isDecimal($('#orderTotal').val()) || !validation.isNotEmpty($('#orderTotal').val())) {
        $('#orderTotal').addClass('error');
    }
    else {
        $('#orderTotal').removeClass('error');
    }
    if (!validation.isNumber($('#zipCode').val()) || !validation.isNotEmpty($('#zipCode').val())) {
        $('#zipCode').addClass('error')
    }
    else {
        $('#zipCode').removeClass('error');
    }
    if ($('#State option:checked').val() == '') {
        $('#State').addClass('error');
    }
    else {
        $('#State').removeClass('error');
    }

    if ($('.error').length) {
        $('.error-text').show();

        // Do not submit form
        return false;
    }
    else {
        $('.error-text').hide();

        // Submit form
        return true;
    }
}

var validation = {
    isNotEmpty: function (str) {
        var pattern = /\S+/;
        return pattern.test(str);  // returns a boolean
    }, 
    isNumber: function (str) {
        var pattern = /^\d+$/;
        return pattern.test(str);  // returns a boolean
    }, 
    isDecimal: function (str) {
        var pattern = /^(\d*\.)?\d+$/;
        return pattern.test(str);  // returns a boolean
    }
};   