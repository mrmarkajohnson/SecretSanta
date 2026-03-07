document.addEventListener('modalOpening', function (e) {
    suggestionsModalOpening(e);
});

document.addEventListener('modalSaveFailed', function (e) {
    suggestionsModalOpening(e);
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