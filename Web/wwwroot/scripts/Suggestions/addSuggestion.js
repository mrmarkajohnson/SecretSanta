window.addEventListener('load', function () {
    initAddSuggestion();
});

document.addEventListener('reloadend', function (e) {
    initAddSuggestion();
});

function initAddSuggestion() {
    let form = document.querySelector('form#suggestionForm');
    initGroupCheckboxes(form);
}