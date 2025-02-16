let dataListStandardPlaceholder = 'Please type or select a value';

function initDataLists() {
    if (isEdge) {
        return true; // not needed for Edge, and can't force Firefox
    }

    let dataListInputs = document.querySelectorAll('input[list]');
    dataListInputs.forEach(handleDataListIssues);
}

function handleDataListIssues(dataListInput) { // ensure the full list is shown even when it has a value    

    let defaultPlaceholder = dataListInput.getAttribute('placeholder');
    if (isEmptyValue(defaultPlaceholder)) {
        defaultPlaceholder = dataListStandardPlaceholder;
    }

    dataListInput.setAttribute('placeholder', defaultPlaceholder);

    ['focus', 'mousedown'].forEach(function (e) {
        dataListInput.addEventListener(e, function () {            
            setPlaceholderAndClearValue();
        });
    });

    dataListInput.addEventListener('blur', function () {
        restoreValueAndPlaceholderIfNeeded();
    });

    dataListInput.addEventListener('input', function () {
        restoreOriginalPlaceholder();
    });

    function setPlaceholderAndClearValue() {
        if (!isEmptyInput(dataListInput)) {
            let currentValue = dataListInput.value;
            dataListInput.setAttribute('placeholder', currentValue);
            dataListInput.value = '';
        } else if (isFirefox) {
            dataListInput.setAttribute('placeholder', defaultPlaceholder + ' (click again to select)');
        }
    }

    function restoreValueAndPlaceholderIfNeeded() {
        let currentPlaceholder = dataListInput.getAttribute('placeholder');
        if (dataListInput.value == '' && currentPlaceholder != defaultPlaceholder) {
            dataListInput.value = currentPlaceholder;
            restoreOriginalPlaceholder();
        }
    }

    function restoreOriginalPlaceholder() {
        dataListInput.setAttribute('placeholder', defaultPlaceholder);
    }
}