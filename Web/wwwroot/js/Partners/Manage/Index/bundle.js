window.addEventListener('load', function () {
    initStatusSelects();
    initDeleteLinks();
});

function initStatusSelects() {
    let statusSelects = document.querySelectorAll('select.relationship-status-select');
    let selectUrl = document.querySelector('div.relationships-table').getAttribute('data-change-url');
    let url = new URL(selectUrl);

    statusSelects.forEach(function (x) {
        x.setAttribute('data-original-value', x.value);

        if (!x.getAttribute('data-initialised')) {
            x.setAttribute('data-initialised', true);

            x.addEventListener('change', function (e) {
                let originalValue = x.getAttribute('data-original-value');

                if (x.value != originalValue) {
                    let name = x.getAttribute('data-name');
                    let headerText = 'Change relationship status';
                    let messageText = 'Are you sure you want to change the status of your relationship with ' + name + '?';

                    if (x.value == 'NotRelationship') { // TODO: More specific wording for other values
                        headerText = 'Deny suggesteed relationship'
                        messageText = 'You\'re NOT in a relationship with ' + name + ', is that correct?';
                    }

                    relationshipStatusChanged(e.currentTarget,
                        url,
                        headerText,
                        messageText);
                }
            });
        }
    });
}

function initDeleteLinks() {
    let deleteLinks = document.querySelectorAll('a.delete-relationship-link');
    let deleteUrl = document.querySelector('div.relationships-table').getAttribute('data-delete-url');
    let url = new URL(deleteUrl);

    deleteLinks.forEach(function (x) {
        if (!x.getAttribute('data-initialised')) {
            x.setAttribute('data-initialised', true);
            x.setAttribute('data-original-value', 'null'); // avoid breaking the relationshipStatusChanged method

            x.addEventListener('click', function (e) {
                let name = x.getAttribute('data-name');
                let headerText = 'Delete relationship';
                let messageText = 'Are you sure you want to delete your proposed relationship with ' + name + '?';

                relationshipStatusChanged(e.currentTarget,
                    url,
                    headerText,
                    messageText);
            });
        }
    });
}

async function relationshipStatusChanged(control, url, title, message) {
    let originalValue = control.getAttribute('data-original-value');

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

            if (result) {
                statusChanged();
            }
            else {
                control.value = originalValue;
            }
        }
    });

    async function statusChanged() {
        let partnerLinkKey = control.getAttribute('data-link-id');
        let hashedUserId = control.getAttribute('data-hashed-user-id');

        url.searchParams.set('partnerLinkKey', partnerLinkKey);
        url.searchParams.set('hashedUserId', hashedUserId);
        url.searchParams.set('newStatus', control.value);

        let response = await fetch(url.href,
            {
                method: "POST",
                redirect: 'follow'
            });

        await response;
        document.dispatchEvent(new Event('ajaxComplete'));
        let responseText = await response.text();

        if (response.redirected) {
            window.location.href = response.url;
        }
        else if (response.ok) {
            control.setAttribute('data-original-value', control.value);
            reloadGrid();
        } else {
            control.value = originalValue;
        }

        if (!response.redirected && responseText != null && responseText != '') {
            if (response.ok) {
                showSuccessMessage(responseText);
            } else {
                showErrorMessage(responseText);
            }
        }
    }
}
document.addEventListener('modalOpening', function (e) {
    relationshipModalOpening(e);
})

document.addEventListener('modalSaved', function (e) {
    relationshipModalSaved(e);
})
async function relationshipModalOpening(e) {
    let modal = e.detail.modal;
    if (modal.id == 'manageRelationshipModal') {
        relationshipModalOpened(modal);
    }
}

function relationshipModalOpened(modal) {
    let nowOptionsSection = modal.querySelector('.now-options-section');

    if (nowOptionsSection) {
        manageNowOptions(modal, nowOptionsSection);
    }
}

function manageNowOptions(modal, nowOptionsSection) {

    let everOptionsSection = modal.querySelector('.ever-options-section');
    let exchangeOptionsSection = modal.querySelector('.exchange-options-section');

    let nowNoOption = nowOptionsSection.querySelector('input[type=radio].radio-no');
    let nowYesOption = nowOptionsSection.querySelector('input[type=radio].radio-yes');
    let nowNotSureOption = nowOptionsSection.querySelector('input[type=radio].radio-not-sure');

    let currentNowOptionYes = nowYesOption.checked;

    if (everOptionsSection) {
        hideSection(everOptionsSection);

        let everNoOption = everOptionsSection.querySelector('input[type=radio].radio-no');
        let everYesOption = everOptionsSection.querySelector('input[type=radio].radio-yes');
        let everNotSureOption = everOptionsSection.querySelector('input[type=radio].radio-not-sure');

        if (exchangeOptionsSection) {
            let exchangeNoOption = exchangeOptionsSection.querySelector('input[type=radio].radio-no');
            let exchangeYesOption = exchangeOptionsSection.querySelector('input[type=radio].radio-yes');
            let exchangeNotSureOption = exchangeOptionsSection.querySelector('input[type=radio].radio-not-sure');

            nowNoOption.addEventListener('click', function() {
                nowNoOrNotSure();
            });

            nowYesOption.addEventListener('click', function() {
                hideSection(everOptionsSection);
                hideSection(exchangeOptionsSection);

                everNoOption.checked = true;
                exchangeNoOption.checked = true;

                currentNowOptionYes = true;
            });

            nowNotSureOption.addEventListener('click', function() {
                nowNoOrNotSure();
            });

            function nowNoOrNotSure() {
                showSection(everOptionsSection);
                showSection(exchangeOptionsSection);

                if (currentNowOptionYes) {
                    resetEverOptions();
                    resetExchangeOptions();
                }

                currentNowOptionYes = false;
            }

            function resetExchangeOptions() {
                exchangeNoOption.checked = exchangeYesOption.checked = exchangeNotSureOption.checked = false;
            }
        } else {
            nowNoOption.addEventListener('click', function() {
                showSection(everOptionsSection);

                if (currentNowOptionYes) {
                    resetEverOptions();
                }

                currentNowOptionYes = false;
            });
            nowYesOption.addEventListener('click', function() {
                hideSection(everOptionsSection);
                everYesOption.checked = true;

                currentNowOptionYes = true;
            });
            nowNotSureOption.addEventListener('click', function() {
                showSection(everOptionsSection);

                if (currentNowOptionYes) {
                    resetEverOptions();
                }

                currentNowOptionYes = false;
            });
        }

        function resetEverOptions() {
            everNoOption.checked = everYesOption.checked = everNotSureOption.checked = false;
        }
    }
    else if (exchangeOptionsSection) {
        let exchangeNoOption = exchangeOptionsSection.querySelector('input[type=radio].radio-no');
        let exchangeYesOption = exchangeOptionsSection.querySelector('input[type=radio].radio-yes');
        let exchangeNotSureOption = exchangeOptionsSection.querySelector('input[type=radio].radio-not-sure');

        nowNoOption.addEventListener('click', function() {
            showSection(exchangeOptionsSection);

            if (currentNowOptionYes) {
                resetExchangeOptions();
            }

            currentNowOptionYes = false;
        });

        nowYesOption.addEventListener('click', function() {
            hideSection(exchangeOptionsSection);
            exchangeNoOption.checked = true;

            currentNowOptionYes = true;
        });

        nowNotSureOption.addEventListener('click', function() {
            showSection(exchangeOptionsSection);

            if (currentNowOptionYes) {
                resetExchangeOptions();
            }

            currentNowOptionYes = false;
        });

        function resetExchangeOptions() {
            exchangeNoOption.checked = exchangeYesOption.checked = exchangeNotSureOption.checked = false;
        }
    }
}

function showSection(section) {
    if (section.classList.contains('collapse')) {
        section.classList.remove('collapse');
    }
}

function hideSection(section) {
    if (!section.classList.contains('collapse')) {
        section.classList.add('collapse');
    }
}

async function relationshipModalSaved(e) {
    let modal = e.detail.modal;
    if (modal.id == 'manageRelationshipModal') {
        reloadGrid();
    }
}