window.addEventListener('load', function () {
    let errorMessageInput = document.querySelector('input.opening-error-message');

    if (!isEmptyInput(errorMessageInput)) {
        showErrorMessage(errorMessageInput.value);
    }
});