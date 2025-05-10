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
function initGroupCheckboxes(form) {
    let groupCheckboxes = form.querySelectorAll('input[type=checkbox].apply-group-checkbox');

    groupCheckboxes.forEach(function (x) {
        initialiseGroupCheckbox(x);
    });

    function initialiseGroupCheckbox(checkbox) {
        if (!checkbox.getAttribute('data-initialised')) {
            checkbox.setAttribute('data-initialised', true);

            checkbox.addEventListener('click', function(e) {
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
                    callback: function(result) {
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