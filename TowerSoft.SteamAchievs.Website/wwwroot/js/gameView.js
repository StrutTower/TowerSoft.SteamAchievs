$(document).ready(function () {
    var gameViewModal = new bootstrap.Modal(document.getElementById('game-view-modal'));

    $('#achievement-tab-pane').on('click', '.edit-achievement-link', function (e) {
        e.preventDefault();
        loadGameViewModal($(this).attr('href'))
    });

    $('#edit-game-details-link').on('click', function (e) {
        e.preventDefault();
        loadGameViewModal($(this).attr('href'))
    });

    $('#game-view-modal').on('submit', '#edit-achievement-form', function (e) {
        e.preventDefault();
        var achievementKey = $(this).data('achievement-key');

        fetch($(this).attr('action'), {
            method: 'post',
            body: new FormData($(this)[0])
        }).then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('.achievement-row[data-achievement-key="' + achievementKey + '"]').replaceWith(data.view);
                    toast(data.message, 'success');

                    gameViewModal.hide();
                } else {
                    toast(data.message, 'danger');
                }
            });
    });

    $('#game-view-modal').on('submit', '#edit-game-details-form', function (e) {
        e.preventDefault();
        fetch($(this).attr('action'), {
            method: 'post',
            body: new FormData($(this)[0])
        }).then(response => response.json())
            .then(data => {
                if (data.success) {
                    $('#game-details-container').html(data.view);
                    toast(data.message, 'success');

                    gameViewModal.hide();
                } else {
                    toast(data.message, 'danger');
                }
            });
    });

    function loadGameViewModal(url) {
        fetch(url)
            .then(response => response.text())
            .then(data => {
                document.getElementById('game-view-modal-body').innerHTML = data;
                gameViewModal.show();

                initializeSlimSelectBasic('#game-view-modal');
            });
    }
});