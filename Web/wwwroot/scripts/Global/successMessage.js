let successMessageUrlStart = 'successMessage=';

function initSuccessMessage() {
    let successMessageInput = document.getElementById('redirectSuccessMessage');

    if (notEmptyInput(successMessageInput)) {
        let successMessage = successMessageInput.value;
        removeSuccessMessageFromUrl(successMessage, false);
        showSuccessMessage(successMessage);
    } else {
        let currentUrl = window.location.href;
        if (currentUrl.includes(successMessageUrlStart)) {
            handleSuccessFromUrl(currentUrl);
        } 
    }
}

function handleSuccessFromUrl(currentUrl) {
    try {
        let nextCharPosition = currentUrl.indexOf(successMessageUrlStart) + successMessageUrlStart.length;
        let remainingUrl = currentUrl.substring(nextCharPosition);
        let encodedSuccessMessage = '';
        let remainingUntilNext = remainingUrl.substring(0, remainingUrl.indexOf('&'));

        if (isEmptyString(remainingUntilNext)) {
            encodedSuccessMessage = remainingUrl;
        } else {
            encodedSuccessMessage = remainingUntilNext;
        }

        removeSuccessMessageFromUrl(encodedSuccessMessage, true);

        try {
            let successMessage = decodeURIComponent(encodedSuccessMessage);
            showSuccessMessage(successMessage);
        } catch {}  // not worth failing over for
    } catch {} // ditto
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
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": 1000,
        "hideDuration": 1000,
        "timeOut": 2000,
        "extendedTimeOut": 100,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    toastr["success"](message);
}
