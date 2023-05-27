$(document).ready(function () {
    // Focus an input with autofocus when opening a modal
    $('.modal').on('shown.bs.modal', function () {
        $(this).find('[autofocus]').focus();
    });

    // Activate disable forms on submit plugin
    $('form[method="post"]').disableOnSubmit({
        buttonTemplate: '<span class="mdi mdi-loading mdi-spin-fast"></span>'
    });
});