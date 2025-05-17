window.addEventListener('load', function () {
    initLinkUserNameAndEmail();
});

function initLinkUserNameAndEmail() {
    let emailInput = document.querySelector('input.email-input');
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
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initNameVariations();
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
    if (!container.getAttribute('data-initialised-ic')) {
        container.setAttribute('data-initialised-ic', true);

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