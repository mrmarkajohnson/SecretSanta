document.addEventListener('modalOpening', function (e) {
    suggestionsModalOpening(e);
});

document.addEventListener('modalSaved', function (e) {
    suggestionsModalSaved(e);
});

async function suggestionsModalOpening(e) {
    let modal = e.detail.modal;

    if (modal.id == 'manageSuggestionModal') {
        suggestionModalOpened(modal);
    }
}

function suggestionModalOpened(modal) {
    let form = modal.querySelector('form');
    initGroupCheckboxes(form);
    initSummernote();
}

async function suggestionsModalSaved(e) {
    let modal = e.detail.modal;

    if (modal.id == 'manageSuggestionModal') {
        reloadGrid();
    }
}