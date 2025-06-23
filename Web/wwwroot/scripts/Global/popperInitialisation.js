function initPopper() {
    setPopovers();
    setTooltips();
}

function setPopovers() {
    $('[data-toggle="popover"]').popover();
}

function setTooltips() {
    $('[data-toggle="tooltip"]').tooltip();
}
