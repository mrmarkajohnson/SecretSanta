window.addEventListener('load', function () {
    initNameVariations();
    initPreferredName();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initNameVariations();
    initPreferredName();
});

function initNameVariations() {
    let showNameContainer = document.querySelector('div.name-variations-container');
    if (showNameContainer) {
        let form = showNameContainer.closest('form');
        if (form) {
            let containerUrl = showNameContainer.getAttribute('data-url');
            if (!isEmptyValue(containerUrl)) {
                let url = new URL(containerUrl);
                let inputs = document.querySelectorAll('input.show-name');
                inputs.forEach(initNameVariation);

                function initNameVariation(input) {
                    if (!input.getAttribute('data-initialised-snv')) {
                        input.setAttribute('data-initialised-snv', true);
                        input.addEventListener('change', showNameVariation);
                    }
                }

                async function showNameVariation() {
                    let data = new FormData(form);

                    let response = await fetch(url.href,
                        {
                            method: "POST",
                            body: data
                        });

                    await response;
                    let responseText = await getResponseText(response);

                    if (isHtml(responseText)) {
                        showNameContainer.innerHTML = responseText;
                    } else {
                        showNameContainer.innerHTML = '';
                    }
                }
            }
        }
    }
}

function initPreferredName() {
    let preferedNameOptions = document.querySelectorAll('input[type=radio].preferred-name-option');
    if (preferedNameOptions) {
        let firstNameInput = document.querySelector('input[type=text].first-name');
        let middleNamesInput = document.querySelector('input[type=text].middle-names');
        let preferredNameInput = document.querySelector('input[type=text].preferred-name');

        if (firstNameInput && middleNamesInput && preferredNameInput) {
            setPreferredName();

            preferedNameOptions.forEach(function (option) {
                if (!option.getAttribute('data-initialised-pf')) {
                    option.setAttribute('data-initialised-pf', true);

                    option.addEventListener('click', function () {
                        preferredOptionSelected(option);
                    });
                }
            });

            function setPreferredName() {
                let selectedOption = Array.from(preferedNameOptions).filter((x) => x.checked)[0];
                preferredOptionSelected(selectedOption, preferredNameInput);
            }

            function preferredOptionSelected(selectedOption) {
                let selectedValue = selectedOption.value;

                if (selectedValue == 'Forename') {
                    preferredNameInput.readOnly = true;
                    trackPreferredOption(firstNameInput);
                    removeTracking(middleNamesInput);
                } else if (selectedValue == 'MiddleName') {
                    preferredNameInput.readOnly = true;
                    trackPreferredOption(middleNamesInput);
                    removeTracking(firstNameInput);
                } else {
                    preferredNameInput.readOnly = false;
                    removeTracking(firstNameInput);
                    removeTracking(middleNamesInput);

                    if (preferredNameInput.value == firstNameInput.value
                        || preferredNameInput.value == middleNamesInput.value) {

                        preferredNameInput.value = '';
                    }
                }
            }

            function trackPreferredOption(input) {
                preferredNameInput.value = input.value;
                input.addEventListener('change', updatePreferredName);
            }

            function removeTracking(input) {
                input.removeEventListener('change', updatePreferredName);
            }

            function updatePreferredName(e) {
                preferredNameInput.value = e.target.value;
            }
        }
    }
}