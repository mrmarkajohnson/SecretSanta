$(function () {
    initPopper();
})

$(document).on('ajaxComplete', function () {
    initPopper();
});

function initPopper() {
    $('[data-toggle="popover"]').popover();
    $('[data-toggle="tooltip"]').tooltip();
}