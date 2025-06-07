window.addEventListener('load', function () {
    initAddSuggestion();
    initSummernote();
});

document.addEventListener('reloadend', function (e) {
    initAddSuggestion();
    initSummernote();
});

function initAddSuggestion() {
    let form = document.querySelector('form#suggestionForm');
    initGroupCheckboxes(form);
}