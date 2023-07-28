$(document).ready(function () {

    $('.search-clear-button').on('click', function (e) {
        $(this).closest('.input-group').find('input').val('').removeClass('input-validation-error').focus();
    });
});