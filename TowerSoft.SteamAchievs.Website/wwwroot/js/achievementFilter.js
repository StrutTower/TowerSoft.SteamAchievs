$(document).ready(function () {
    var achievementFilter = new SlimSelect({
        select: document.getElementById('achievement-filter'),
        settings: {
            placeholderText: 'Filter Achievements',
        },
        events: {
            afterChange: () => {
                filterAchievements();
            }
        }
    });

    $('#achieved-filter').on('change', function (e) {
        filterAchievements();
    });

    function filterAchievements() {
        var tagIDs = achievementFilter.getSelected();
        var achievedFilter = $('#achieved-filter').val();

        if (tagIDs.length == 0 && achievedFilter === '') {
            document.querySelectorAll('.achievement-row').forEach(function (item) {
                item.classList.remove('hidden');
            });
        } else {
            var enableTagFilter = tagIDs.length > 0;
            var enableCompletionFilter = achievedFilter.length > 0;

            document.querySelectorAll('.achievement-row').forEach(function (item) {
                var showByCompletion = false;
                var showByTag = false;

                if (enableTagFilter) {
                    tagIDs.forEach(function (tagID) {
                        if (item.dataset.tags.indexOf(';' + tagID + ';') >= 0) {
                            showByTag = true;
                        }
                    });
                }

                if (enableCompletionFilter && item.dataset.achieved === achievedFilter) {
                    showByCompletion = true;
                }

                if (enableTagFilter && enableCompletionFilter) {
                    if (showByCompletion && showByTag) {
                        item.classList.remove('hidden');
                    } else {
                        item.classList.add('hidden');
                    }
                } else if (enableTagFilter && showByTag) {
                    item.classList.remove('hidden');
                } else if (enableCompletionFilter && showByCompletion) {
                    item.classList.remove('hidden');
                } else {
                    item.classList.add('hidden');
                }
            });
        }
    }
})