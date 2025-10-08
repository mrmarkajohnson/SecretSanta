window.addEventListener('load', function () {
    initialiseGiftingGroupEdit();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initialiseGiftingGroupEdit();
});

function initialiseGiftingGroupEdit() {
    let cultureSelect = document.querySelector('select.culture-info-select');
    if (cultureSelect) {
        let currencyOverrideInput = document.querySelector('input.default-currency');

        if (currencyOverrideInput && !initialised(currencyOverrideInput, 'group-edit')) {
            cultureSelect.addEventListener('change', function () {
                let selectedOption = cultureSelect.options[cultureSelect.selectedIndex];
                currencyOverrideInput.value = selectedOption.getAttribute('data-currency-string');
            });
        }
    }
}

document.addEventListener('modalOpening', function (e) {
    sendInvitationModalOpening(e);
});

async function sendInvitationModalOpening(e) {
    let modal = e.detail.modal;
    if (modal.id == 'sendInvitationModal') {
        initUserSelect();
    }
}

document.addEventListener('reloadend', function (e) {
    initUserSelect();
});

function initUserSelect() {
    let selectRadios = document.querySelectorAll('input.user-select');
    let toHashedUserIdInput = document.querySelector('input#toHashedUserId');

    selectRadios.forEach(function (radio) {
        if (!initialised(radio, 'status-select')) {

            radio.setAttribute('data-checked', radio.checked);

            radio.addEventListener('click', function (e) {
                let isChecked = radio.getAttribute('data-checked');
                let tooltip = '';

                if (isTrueValue(isChecked)) {
                    radio.checked = false;
                    toHashedUserIdInput.value = '';
                    tooltip = "Select";
                } else {                    
                    let toHashedUserId = radio.getAttribute('data-hashed-user-id');
                    toHashedUserIdInput.value = toHashedUserId;
                    tooltip = "Click again to clear";
                }

                radio.setAttribute('data-checked', radio.checked);
                
                $(radio).tooltip('hide');
                radio.setAttribute('data-bs-original-title', tooltip);

                $.when().then(function() {
                    $(radio).tooltip('update');
                    $(radio).tooltip('show');
                });
            });
        }
    });
}