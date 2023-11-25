$(document).ready(function () {
    var showTagFilter = new SlimSelect({
        select: document.getElementById('achievement-filter'),
        settings: {
            placeholderText: 'Show Tags',
        },
        events: {
            afterChange: () => {
                filterAchievements();
            }
        }
    });

    var hideTagFilter = new SlimSelect({
        select: document.getElementById('hide-tag-filter'),
        settings: {
            placeholderText: 'Hide Tags'
        },
        events: {
            afterChange: () => {
                filterAchievements();
            }
        }
    })

    $('#achieved-filter').on('change', function (e) {
        filterAchievements();
    });

    function filterAchievements() {
        var showTagIDs = showTagFilter.getSelected();
        var hideTagIDs = hideTagFilter.getSelected();
        var achievedFilter = $('#achieved-filter').val();

        if (showTagIDs.length == 0 && hideTagIDs.length == 0 && achievedFilter === '') {
            document.querySelectorAll('.achievement-row').forEach(function (item) {
                item.classList.remove('hidden');
            });
        } else {
            var enableShowTagFilter = showTagIDs.length > 0;
            var enableHideTagFilter = hideTagIDs.length > 0;
            var enableCompletionFilter = achievedFilter.length > 0;

            if (enableShowTagFilter && enableHideTagFilter) {
                document.getElementById('hide-filter-overridden').classList.remove('hidden');
            } else {
                document.getElementById('hide-filter-overridden').classList.add('hidden');
            }

            document.querySelectorAll('.achievement-row').forEach(function (item) {
                let showByCompletion = false;
                let showByTag = false;
                let hideByTag = false;

                if (enableShowTagFilter) {
                    showTagIDs.forEach(function (tagID) {
                        if (tagID === 'BLANK' && item.dataset.tags === ';') {
                            showByTag = true;
                        } else if (item.dataset.tags.indexOf(';' + tagID + ';') >= 0) {
                            showByTag = true;
                        }
                    });
                }

                if (enableHideTagFilter) {
                    hideTagIDs.forEach(function (tagID) {
                        if (tagID === 'BLANK' && item.dataset.tags === ';') {
                            hideByTag = true;
                        } else if (item.dataset.tags.indexOf(';' + tagID + ';') >= 0) {
                            hideByTag = true;
                        }
                    });
                }

                if (enableCompletionFilter && item.dataset.achieved === achievedFilter) {
                    showByCompletion = true;
                }

                let show = false;
                if (enableShowTagFilter && enableCompletionFilter) {
                    if (showByCompletion && showByTag) {
                        show = true;
                    }
                } else if (enableShowTagFilter) {
                    if (showByTag) {
                        show = true;
                    }
                } else if (enableCompletionFilter && enableHideTagFilter) {
                    if (showByCompletion && !hideByTag) {
                        show = true;
                    }
                } else if (enableHideTagFilter && !hideByTag) {
                    show = true;
                } else if (enableCompletionFilter && showByCompletion) {
                    show = true;
                }

                if (show) {
                    item.classList.remove('hidden');
                } else {
                    item.classList.add('hidden');
                }
            });
        }
    }
})