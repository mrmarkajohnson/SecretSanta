function initPopper() {
    setPopovers();
    setTooltips();
}

function setPopovers() {
    $('[data-toggle="popover"]').each(function (i, e) {
        try {
            $(e).popover();
        } catch { }
    });
}

function setTooltips() {
    $('[data-toggle="tooltip"]').each(function (i, e) {
        try {
            $(e).tooltip();
        } catch { }
    });
}
