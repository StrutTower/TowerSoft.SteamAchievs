﻿/**
 * @param {string} text - Toast message
 * @param {string} type -  Bootstrap color type. Example: primary, secondary, success, warning, danger, info
 * @param {number} duration - Optional. number of milliseconds to show the toast. Default is 5000. 
 */
function toast(text, type, duration) {
    generateToast(false, text, type, duration);
}

/**
 * @param {string} text - Toast message
 * @param {string} type -  Bootstrap color type. Example: primary, secondary, success, warning, danger, info
 * @param {number} duration - Optional. number of milliseconds to show the toast. Default is 5000. 
 */
function toastHtml(text, type, duration) {
    generateToast(true, text, type, duration);
}

/**
 * @param {boolean} isHtml - Determines if the text should be rendered as HTML
 * @param {string} text - Toast message
 * @param {string} type -  Bootstrap color type. Example: primary, secondary, success, warning, danger, info
 * @param {number} duration - Optional. number of milliseconds to show the toast. Default is 5000. 
 */
function generateToast(isHtml, text, type, duration) {
    if (duration === undefined) {
        duration = 5000;
    }

    var template = document.getElementById('toast-template').querySelector('.toast').cloneNode(true);

    var container = document.querySelector('.toast-container');
    container.appendChild(template);
    if (isHtml) {
        $(template).find('.toast-body').html(text);
    } else {
        template.querySelector('.toast-body').innerText = text;
    }

    if (type !== undefined && type.length > 0) {
        var header = template.querySelector('.toast-header');
        header.classList.remove('.bg-primary');
        header.classList.add(type);
    }

    var toast = new bootstrap.Toast(template, {
        delay: duration
    });
    toast.show();

    setTimeout(function () {
        container.removeChild(template);
    }, duration + 2000);
}

$(document).ready(function () {
    var tempDataMessage = document.getElementById('tempDataMessage');
    if (tempDataMessage && tempDataMessage.value) {
        toast(tempDataMessage.value, 'bg-info');
    }
}, false);