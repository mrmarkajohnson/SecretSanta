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
function initGroupCheckboxes(form) {
    let groupCheckboxes = form.querySelectorAll('input[type=checkbox].apply-group-checkbox');

    groupCheckboxes.forEach(function (x) {
        initialiseGroupCheckbox(x);
    });

    function initialiseGroupCheckbox(checkbox) {
        if (!initialised(checkbox, 'participate')) {
            checkbox.addEventListener('click', function (e) {
                groupCheckboxClicked(checkbox);
            });
        }
    }

    function groupCheckboxClicked(checkbox) {
        let checked = checkbox.checked;

        if (checked) {
            let included = isTrueValue(checkbox.getAttribute('data-included'));

            if (!included) {
                let group = checkbox.getAttribute('data-group');
                let thisGroup = group != null ? 'group \'' + group + '\'' : 'this group';
                let title = 'Participate in ' + thisGroup + '?';
                let message = 'You aren\'t currently participating in ' + thisGroup + ' this year. Would you like to?';

                bootbox.confirm({
                    title: title,
                    message: message,
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-no'
                        }
                    },
                    callback: function (result) {
                        bootbox.hideAll(); // avoid issues with the bootbox not closing the second time it's opened

                        if (!result) {
                            checkbox.checked = false;
                        }
                    }
                });
            }
        }
    }
}