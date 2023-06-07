$(document).ready(function () {
    $('#game-filter').on('change', function (e) {
        var value = $(this).val();

        if (value === '') {
            $('.tag-list-item').show();
        } else {
            $('.tag-list-item').hide();
            $('.tag-list-item[data-game="' + value + '"]').show();
        }
    });
});