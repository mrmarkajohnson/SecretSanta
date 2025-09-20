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
