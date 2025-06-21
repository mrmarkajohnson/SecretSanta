window.addEventListener('load', function () {
    initSendMessage();
});

document.addEventListener('reloadend', function (e) {
    initSendMessage();
});