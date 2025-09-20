window.addEventListener('load', function () {
    initLinkUserNameAndEmail();
});

function initLinkUserNameAndEmail() {
    let emailInput = document.querySelector('input.email-input');

    if (initialised(emailInput, 'user-name'))
        return;

    let userNameContainer = document.querySelector('div.username-container');
    let userNameInput = userNameContainer.querySelector('input.username-input');
    let useEmailCheckboxContainer = document.querySelector('div.use-email-checkbox-container');
    let useEmailCheckbox = useEmailCheckboxContainer.querySelector('input.use-email-checkbox');
    let useEmail = false;

    emailInput.addEventListener('change', emailChanged);
    useEmailCheckbox.addEventListener('change', useEmailCheckboxChanged);
    
    setInitialLink();

    function setInitialLink() {
        let userName = userNameInput.value;
        let email = emailInput.value;
        if (notEmptyValue(email) && userName == email) {
            useEmail = true;
            toggleUseEmailContainer(true);
            useEmailCheckbox.checked = true;
            userNameInput.addEventListener('change', userNameChanged);
        }
    }

    function toggleUseEmailContainer(show) {
        if (show) {
            useEmailCheckboxContainer.setAttribute('style', 'display:block !important');
            userNameContainer.classList.remove('col-sm-12');
            userNameContainer.classList.add('col-sm-9', 'col-8');
        } else {
            useEmailCheckboxContainer.setAttribute('style', 'display:none !important');
            userNameContainer.classList.add('col-sm-12');
            userNameContainer.classList.remove('col-sm-9', 'col-8');
        }
    }

    function emailChanged() {
        let email = emailInput.value;
        if (notEmptyValue(email)) {
            toggleUseEmailContainer(true);
            if (useEmail) {
                updateUserNameFromEmail();
            }
        } else {
            toggleUseEmailContainer(false);
        }
    }

    function useEmailCheckboxChanged() {
        useEmail = useEmailCheckbox.checked;
        if (useEmail) {
            updateUserNameFromEmail();
            userNameInput.addEventListener('change', userNameChanged);
        } else {
            userNameInput.removeEventListener('change', userNameChanged);
            let userName = userNameInput.value;
            let email = emailInput.value;
            if (userName == email) {                
                userNameInput.value = '';
                userNameInput.dispatchEvent(new Event('blur'));
            }
        }
    }

    function updateUserNameFromEmail() {
        let email = emailInput.value;
        if (notEmptyValue(email)) {
            userNameInput.value = email;
            userNameInput.dispatchEvent(new Event('blur'));
        }
    }

    function userNameChanged() {
        if (useEmail) {
            let userName = userNameInput.value;
            let email = emailInput.value;
            if (userName != email) {
                useEmailCheckbox.checked = false;
                userNameInput.removeEventListener('change', userNameChanged);
            }
        }
    }
}

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
                    if (!initialised(input, 'snv')) {
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
                if (!initialised(option, 'preferred')) {
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
window.addEventListener('load', function () {
    initInputsWithCheckbox();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initInputsWithCheckbox();
});

function initInputsWithCheckbox() {
    let containers = document.querySelectorAll('.input-with-checkbox-container');
    containers.forEach(initInputWithCheckbox);
}

function initInputWithCheckbox(container) {
    if (!initialised(container, 'input-checkbox')) {
        let textInput = container.querySelector('input[type=text]');
        let checkbox = container.querySelector('input[type=checkbox]');

        if (textInput && checkbox) {

            let textContainer = textInput.closest('div.form-group-ib');
            let checkboxContainer = checkbox.closest('div.form-group-ib');

            if (textContainer && checkboxContainer) {

                toggleCheckboxContainer();

                textInput.addEventListener('change', function () {
                    toggleCheckboxContainer();
                });

                function toggleCheckboxContainer() {
                    let show = !isEmptyInput(textInput);

                    if (show) {
                        checkboxContainer.setAttribute('style', 'display:block !important');
                        textContainer.classList.remove('col-sm-12');
                        textContainer.classList.add('col-sm-9', 'col-8');
                    } else {
                        checkboxContainer.setAttribute('style', 'display:none !important');
                        textContainer.classList.add('col-sm-12');
                        textContainer.classList.remove('col-sm-9', 'col-8');
                        checkbox.checked = false;
                    }
                }
            }
        }
    }
}