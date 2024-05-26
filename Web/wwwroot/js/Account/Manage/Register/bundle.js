window.addEventListener("load", function () {
    initRegisterPage();
});

function initRegisterPage() {
    let emailInput = document.querySelector('input.email-input');
    let userNameInput = document.querySelector('input.username-input');
    let useEmailCheckboxContainer = document.querySelector('div.use-email-checkbox-container');
    let useEmailCheckbox = useEmailCheckboxContainer.querySelector('input.use-email-checkbox');
    let useEmail = false;

    emailInput.addEventListener('change', emailChanged);
    useEmailCheckbox.addEventListener('change', useEmailCheckboxChanged);

    function emailChanged() {
        let email = emailInput.value;
        if (notEmptyValue(email)) {
            useEmailCheckboxContainer.style.display = 'block';
            if (useEmail) {
                updateUserNameFromEmail();
            }
        } else {
            useEmailCheckboxContainer.style.display = 'none';
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

function notEmptyValue(value) {
    return value != undefined && value != null && value != '';
}
