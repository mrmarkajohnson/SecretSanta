let successMessageUrlStart = 'successMessage=';

function initSuccessMessage() {
    let successMessage = getSuccessMessage();

    if (!isEmptyValue(successMessage)) {
        removeSuccessMessageFromUrl(successMessage, false);
        showSuccessMessage(successMessage);
    } else {
        let currentUrl = window.location.href;
        if (currentUrl.includes(successMessageUrlStart)) {
            handleSuccessFromUrl(currentUrl);
        }
    }
}

function getSuccessMessage() {
    let successMessageInput = document.getElementById('successMessage');
    let successMessage = notEmptyInput(successMessageInput) ? successMessageInput.value : '';
    return successMessage;
}

function handleSuccessFromUrl(currentUrl) {
    try {
        let nextCharPosition = currentUrl.indexOf(successMessageUrlStart) + successMessageUrlStart.length;
        let remainingUrl = currentUrl.substring(nextCharPosition);
        let encodedSuccessMessage = '';
        let remainingUntilNext = remainingUrl.substring(0, remainingUrl.indexOf('&'));

        if (isEmptyValue(remainingUntilNext)) {
            encodedSuccessMessage = remainingUrl;
        } else {
            encodedSuccessMessage = remainingUntilNext;
        }

        removeSuccessMessageFromUrl(encodedSuccessMessage, true);

        try {
            let successMessage = decodeURIComponent(encodedSuccessMessage);
            showSuccessMessage(successMessage);
        } catch {
            console.log('Showing success message failed'); // not worth failing over for
        }
    } catch {
        console.log('Success message handling failed'); // ditto
    }
}

function removeSuccessMessageFromUrl(message, alreadyEncoded) {
    let currentUrl = window.location.href;
    let encodedSuccess = successMessageUrlStart + (alreadyEncoded ? message : encodeURIComponent(message));

    try {
        if (currentUrl.includes(encodedSuccess)) {
            if (currentUrl.includes('?' + encodedSuccess)) {
                currentUrl = currentUrl.replace('?' + encodedSuccess, '');
                window.history.replaceState({}, null, currentUrl);
            } else if (currentUrl.includes('&' + encodedSuccess)) {
                currentUrl = currentUrl.replace('&' + encodedSuccess, '');
                window.history.replaceState({}, null, currentUrl);
            }
        }
    } catch { } // not worth failing over for
}

function showSuccessMessage(message) {
    setToastrOptions();
    toastr["success"](message);
}
