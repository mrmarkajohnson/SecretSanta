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
