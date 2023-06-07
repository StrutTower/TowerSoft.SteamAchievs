$(document).ready(function () {
    initializeSlimSelectBasic();
});

function initializeSlimSelectBasic(parentSelector) {
    var selector = '.slim-select-basic';
    if (parentSelector !== undefined) {
        selector = parentSelector + ' .slim-select-basic'
    }

    document.querySelectorAll(selector).forEach(function (element) {
        new SlimSelect({
            select: element
        });
    });
}