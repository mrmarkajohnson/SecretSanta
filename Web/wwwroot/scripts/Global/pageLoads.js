let isFirefox = navigator.userAgent.toLowerCase().includes('firefox');
let isEdge = navigator.userAgent.toLowerCase().includes('edge');

window.addEventListener('load', function () {
    initPopper();
    initSuccessMessage();
    initEyeSymbols();
    initDataLists()
    initThinking();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initPopper();
    initEyeSymbols();
    initDataLists()
    initThinking();
});