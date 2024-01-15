$(document).ready(function () {
    var sortSelects = $('.twr-sort-select');
    sortSelects.each(function () {
        var storedSort = sessionStorage.getItem(this.dataset.sortStorageKey);
        if (storedSort !== null)
            this.value = storedSort;

        sort(this);

        $(this).on('change', function () {
            sort(this);
            sessionStorage.setItem(this.dataset.sortStorageKey, this.value);
        });
    });

    function sort(ele) {
        let sortType = ele.value;
        let containerTarget = ele.dataset.containerTarget;
        let itemTarget = ele.dataset.itemTarget;


        let items = document.querySelectorAll(containerTarget + ' ' + itemTarget);
        let itemsArray = Array.prototype.slice.call(items, 0);

        switch (sortType) {
            case 'nameAZ':
                itemsArray = itemsArray.sort(nameAZ);
                break;
            case 'nameZA':
                itemsArray = itemsArray.sort(nameZA);
                break;
            case 'countSortAsc':
                itemsArray = itemsArray.sort(countSortAsc);
                break;
            case 'countSortDesc':
                itemsArray = itemsArray.sort(countSortDesc);
                break;
            case 'percentSortAsc':
                itemsArray = itemsArray.sort(percentSortAsc);
                break;
            case 'percentSortDesc':
                itemsArray = itemsArray.sort(percentSortDesc);
                break;
            case 'playtimeSortAsc':
                itemsArray = itemsArray.sort(playtimeSortAsc);
                break;
            case 'playtimeSortDesc':
                itemsArray = itemsArray.sort(playtimeSortDesc);
                break;
            case 'reviewSortAsc':
                itemsArray = itemsArray.sort(reviewSortAsc);
                break;
            case 'reviewSortDesc':
                itemsArray = itemsArray.sort(reviewSortDesc);
                break;
            case 'metacriticScoreAsc':
                itemsArray = itemsArray.sort(metacriticScoreAsc);
                break;
            case 'metacriticScoreDesc':
                itemsArray = itemsArray.sort(metacriticScoreDesc);
                break;
            case 'playNextSortAsc':
                itemsArray = itemsArray.sort(playNextSortAsc);
                break;
            case 'playNextSortDesc':
                itemsArray = itemsArray.sort(playNextSortDesc);
                break;
            case 'mainStoryTimeAsc':
                itemsArray = itemsArray.sort(mainStoryTimeSortAsc);
                break;
            case 'mainStoryTimeDesc':
                itemsArray = itemsArray.sort(mainStoryTimeSortDesc);
                break;
            case 'completionistTimeAsc':
                itemsArray = itemsArray.sort(completionistTimeSortAsc);
                break;
            case 'completionistTimeDesc':
                itemsArray = itemsArray.sort(completionistTimeSortDesc);
                break;
            case 'releasedAsc':
                itemsArray = itemsArray.sort(releasedSortAsc);
                break;
            case 'releasedDesc':
                itemsArray = itemsArray.sort(releasedSortDesc);
                break;
        }

        let container = document.querySelector(containerTarget);
        for (let x = 0; x < itemsArray.length; x++) {
            container.appendChild(itemsArray[x]);
        }
    }

    function nameAZ(a, b) {
        return b.dataset.name.toUpperCase() < a.dataset.name.toUpperCase() ? 1 : -1;
    }
    function nameZA(a, b) {
        return b.dataset.name.toUpperCase() < a.dataset.name.toUpperCase() ? -1 : 1;
    }
    function countSortAsc(a, b) {
        return +a.dataset.count - +b.dataset.count;
    }
    function countSortDesc(a, b) {
        return +b.dataset.count - +a.dataset.count;
    }
    function percentSortAsc(a, b) {
        return +a.dataset.percent - +b.dataset.percent;
    }
    function percentSortDesc(a, b) {
        return +b.dataset.percent - +a.dataset.percent;
    }
    function playtimeSortAsc(a, b) {
        return +a.dataset.playtime - +b.dataset.playtime;
    }
    function playtimeSortDesc(a, b) {
        return +b.dataset.playtime - +a.dataset.playtime;
    }
    function reviewSortAsc(a, b) {
        return +a.dataset.review - +b.dataset.review;
    }
    function reviewSortDesc(a, b) {
        return +b.dataset.review - +a.dataset.review;
    }
    function metacriticScoreAsc(a, b) {
        return +a.dataset.metacritic - +b.dataset.metacritic;
    }
    function metacriticScoreDesc(a, b) {
        return +b.dataset.metacritic - +a.dataset.metacritic;
    }
    function playNextSortAsc(a, b) {
        return +a.dataset.playnext - +b.dataset.playnext;
    }
    function playNextSortDesc(a, b) {
        return +b.dataset.playnext - +a.dataset.playnext;
    }
    function mainStoryTimeSortAsc(a, b) {
        return +a.dataset.mainstorytime - +b.dataset.mainstorytime;
    }
    function mainStoryTimeSortDesc(a, b) {
        return +b.dataset.mainstorytime - +a.dataset.mainstorytime;
    }
    function completionistTimeSortAsc(a, b) {
        return +a.dataset.completionisttime - +b.dataset.completionisttime;
    }
    function completionistTimeSortDesc(a, b) {
        return +b.dataset.completionisttime - +a.dataset.completionisttime;
    }
    function releasedSortAsc(a, b) {
        return +a.dataset.released - +b.dataset.released;
    }
    function releasedSortDesc(a, b) {
        return +b.dataset.released - +a.dataset.released;
    }
});