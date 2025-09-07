let isFirefox = navigator.userAgent.toLowerCase().includes('firefox');
let isEdge = navigator.userAgent.toLowerCase().includes('edge');

window.addEventListener('load', function () {
    initAjaxComplete();
    initSuccessMessage();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initAjaxComplete();
});

document.addEventListener('reloadend', function (e) {
    initAlwaysReload();
});

// these always load after ajaxComplete is called, or when the page load
function initAjaxComplete() {
    initAlwaysReload();
    initEyeSymbols();
    initDataLists();
    initThinking();
}

// these always load after any page load or refresh
function initAlwaysReload() {
    initPopper();
    initModalLinks();
    initDeleteLinks();
    initBackgroundLinks();
}
