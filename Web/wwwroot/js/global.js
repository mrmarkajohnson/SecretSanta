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
function showErrorMessage(message) {
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

    toastr["error"](message);
}
async function submitFormViaFetch(form, url) {
    if (isEmptyValue(url)) {
        url = form.getAttribute('action');
    }

    let data = new FormData(form);

    let response = await fetch(url,
        {
            method: "POST",
            body: data,
            redirect: 'follow'
        });

    let responseClone = response.clone(); // this allows us to use response.text() on the return result; it can only be called once

    if (response.status > 400) {
        showErrorMessage('Unknown error, please contact an administrator');
    }
    else if (response.redirected) {
        window.location.href = response.url;
    }
    else {
        let responseText = await getResponseText(response);

        if (isHtml(responseText)) {
            if (responseText.includes('<form')) {
                let parser = new DOMParser();
                let responseHtml = parser.parseFromString(responseText, "text/html");
                let responseForm = responseHtml.querySelector('form');
                form.innerHTML = responseForm.innerHTML;
            }
            else {
                form.innerHTML = responseText;
            }
        }
    }

    document.dispatchEvent(new Event('ajaxComplete'));
    return responseClone;
}

document.addEventListener('reloadstart', function (e) {
    //console.log('grid: ', e.detail.grid);
    let gridId = e.detail.grid.element.id;
    let grid = document.getElementById(gridId);

    if (!!grid && isEmptyValue(grid.innerHTML)) {
        grid.insertAdjacentHTML('afterbegin', '<i>Loading, please wait...</i>');
    }
});

document.addEventListener('click', function (e) {
    try {
        if (e.target && e.target.classList && e.target.classList.contains('grid-content-refresh')) {
            let gridElement = e.target.closest('.mvc-grid');

            if (gridElement) {
                let grid = new MvcGrid(gridElement);

                //grid.requestType = 'post'; // defaults to get
                //grid.query.set('name', 'Joe');
                grid.reload();
            }
        }
    } catch {
        console.log('Grid reload failed');
    }
});

function reloadGrid() {
    let grid = new MvcGrid(document.querySelector('.mvc-grid'));
    grid.reload();
}
function initModalLinks() {
    let modalLinks = document.querySelectorAll('a.modal-link');
    modalLinks.forEach(initModalLink);
}

function initModalLink(modalLink) {
    if (!modalLink.getAttribute('data-initialised')) {
        modalLink.setAttribute('data-initialised', true);

        modalLink.addEventListener('click', function () {
            showModal(modalLink);
        });
    }
}

async function showModal(modalLink) {
    let linkUrl = modalLink.getAttribute('data-url');
    let url = new URL(linkUrl);

    let response = await fetch(url.href,
        {
            method: "GET",
        });

    await response;    

    let responseText = await response.text();

    if (response.ok) {
        showModalResponse(responseText);
        document.dispatchEvent(new Event('ajaxComplete'));
    } else {
        showErrorMessage(responseText);
    }
}

function showModalResponse(responseText) {
    let modalContainer = document.getElementById('modalContainer');

    if (modalContainer) {
        modalContainer.removeEventListener('hidden.bs.modal', modalClosed); // prevent previous versions from throwing closure events
        modalContainer.innerHTML = responseText;
    }
    else {
        document.body.insertAdjacentHTML('afterbegin', '<div id="modalContainer">' + responseText + '</div>');
        modalContainer = document.getElementById('modalContainer');
    }

    let modal = modalContainer.querySelector('div.modal');
    let modalObject = new bootstrap.Modal(modal);

    modalContainer.addEventListener('hidden.bs.modal', modalClosed);
    document.dispatchEvent(new CustomEvent('modalOpening', { detail: { modal: modal } }));

    let saveButton = modal.querySelector('.btn-save-close-modal');
    if (!!saveButton) {
        saveButton.addEventListener('click', function () {
            saveModalForm(modal, modalObject);
        });
    }

    modalObject.show();
}

function modalClosed(e) {
    let modal = e.target;

    if (modal) {
        if (document.activeElement) {
            document.activeElement.blur(); // avoid annoying 'Blocked aria-hidden on an element...' message
        }

        document.dispatchEvent(new CustomEvent('modalClosed', { detail: { modal: modal } }));
        document.querySelectorAll('.modal-backdrop').forEach(function (x) {
            x.remove();
        });
    }
}

async function saveModalForm(modal, modalObject) {
    let form = modal.querySelector('form');
    let response = await submitFormViaFetch(form);
    let responseText = await getResponseText(response);

    if (response.ok) {
        if (isHtml(responseText)) {
            let successMessage = getSuccessMessage();

            if (!isEmptyValue(successMessage)) {
                handleSuccessfulSave(successMessage);
            }
        }
        else {
            if (!isEmptyValue(responseText)) {
                handleSuccessfulSave(responseText);
            }

            modalObject.hide();
        }
    } else if (!isEmptyValue(responseText) && !isHtml(responseText)) {
        showErrorMessage(responseText);
    }

    function handleSuccessfulSave(message) {
        showSuccessMessage(message);
        modalObject.hide();
        document.dispatchEvent(new CustomEvent('modalSaved', { detail: { modal: modal } }));
    }
}

let isFirefox = navigator.userAgent.toLowerCase().includes('firefox');
let isEdge = navigator.userAgent.toLowerCase().includes('edge');

window.addEventListener('load', function () {
    initPopper();
    initSuccessMessage();
    initEyeSymbols();
    initDataLists()
    initThinking();
    initModalLinks();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initPopper();
    initEyeSymbols();
    initDataLists()
    initThinking();
    initModalLinks();
});

document.addEventListener('reloadend', function (e) {
    initPopper();
    initModalLinks();
});
function initPopper() {
    $('[data-toggle="popover"]').popover();
    $('[data-toggle="tooltip"]').tooltip();
}
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

let submitTimer = null;

function initThinking() {
    let thinkingSection = document.querySelector('.thinking');
    clearThinkingElements(thinkingSection, null);

    window.addEventListener('beforeunload', function () {
        clearThinkingElements(thinkingSection, null);
        showThinkingAnimation(thinkingSection, null);
    });

    var forms = document.querySelectorAll('form');
    forms.forEach(initSubmitThinking);

    function initSubmitThinking(form) {
        let $form = $(form); // .valid() is a JQuery extension
        let submitButton = null;        

        form.addEventListener('submit', function (e) {
            
            if (typeof $.fn.valid != 'function' || $form.valid()) {
                submitButton = e.currentTarget;
                submitButton.setAttribute('disabled', 'disabled');
                showThinkingAnimation(thinkingSection, submitButton);
            }
        }, { passive: true });

        $('form').on('invalid-form.validate', function () { // need JQuery to get this
            clearThinkingElements(thinkingSection, submitButton);            
        });
    }
}

function showThinkingAnimation(thinkingSection, submitButton) {
    submitTimer = setTimeout(function() {
        thinkingSection.style.display = "block";
        if (submitButton) {
            submitButton.removeAttribute('disabled');
        }
    }, 200);
}

function clearThinkingElements(thinkingSection, submitButton) {
    if (submitTimer) {
        clearTimeout(submitTimer);
    }
    thinkingSection.style.display = "none";
    if (submitButton) {
        submitButton.removeAttribute('disabled');
    }
}

async function selectUser(radio, url) {
    let selectedUserId = radio.getAttribute('data-user-id');
    url.searchParams.set('globalUserId', selectedUserId);

    let response = await fetch(url.href,
    {
        method: "POST",
        redirect: 'follow'
    });

    await response;
    document.dispatchEvent(new Event('ajaxComplete'));
    let responseText = await response.text();

    if (response.redirected) {
        window.location.href = response.url;
    }
    else if (responseText != null && responseText != '') {
        if (response.ok) {
            showSuccessMessage(responseText);
        } else {
            showErrorMessage(responseText);
        }
    }
}
let hideTextClass = 'hide-text';

function initEyeSymbols() {
    let eyeSymbols = document.querySelectorAll('.fa-eye');
    eyeSymbols.forEach(initEyeSymbol);
}

function initEyeSymbol(eyeSymbol) {
    let inputGroup = eyeSymbol.closest('.input-group');
    if (inputGroup) {
        let input = inputGroup.querySelector('input[type=password], input.' + hideTextClass);
        if (input) {
            let isPassword = input.type == 'password';
            let showText = false;
            let noEyeSymbol = inputGroup.querySelector('.fa-eye-slash');;

            let eventList = ["change", "keyup", "paste", "input"];
            for (e of eventList) {
                input.addEventListener(e, toggleEyeSymbol);
            }

            addToggleListener(eyeSymbol);

            input.addEventListener('blur', function () {
                showText = false;
                toggleInputType(input, isPassword, showText);
                
                eyeSymbol.classList.add('collapse');
                if (noEyeSymbol) {
                    noEyeSymbol.classList.add('collapse');
                }
            });

            input.addEventListener('focus', function () {
                if (!isEmptyInput(input) && !showText) {
                    eyeSymbol.classList.remove('collapse');
                    if (noEyeSymbol) {
                        noEyeSymbol.classList.add('collapse');
                    }
                }                
            });

            function toggleEyeSymbol() {
                if (isEmptyInput(input)) {
                    if (!showText) {
                        eyeSymbol.classList.add('collapse');
                    } else if (noEyeSymbol) {
                        noEyeSymbol.classList.add('collapse');
                    }
                } else {
                    if (!showText) {
                        eyeSymbol.classList.remove('collapse');
                    } else if (noEyeSymbol) {
                        noEyeSymbol.classList.remove('collapse');
                    }
                }
            }

            function togglePassword() {
                if (showText) {
                    convertToPassword();
                } else {
                    convertToText();
                }

                return false;
            }

            function convertToPassword() {
                showText = false;
                toggleInputType(input, isPassword, showText);

                eyeSymbol.classList.remove('collapse');
                if (noEyeSymbol) {
                    noEyeSymbol.classList.add('collapse');
                }
            }

            function convertToText() {
                showText = true;
                toggleInputType(input, isPassword, showText);

                eyeSymbol.classList.add('collapse');
                if (noEyeSymbol) {
                    noEyeSymbol.classList.remove('collapse');
                } else {
                    noEyeSymbol = document.createElement('i');
                    noEyeSymbol.setAttribute('class', 'fa fa-eye-slash');

                    let inputGroupText = eyeSymbol.closest('.input-group-text');
                    inputGroupText.append(noEyeSymbol);

                    addToggleListener(noEyeSymbol);
                }
            }

            function addToggleListener(symbol) {
                symbol.addEventListener('mousedown', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    togglePassword();
                });
            }
        }
    }

    function toggleInputType(input, isPassword, showText) {
        if (isPassword) {
            input.type = showText ? 'text' : 'password';
        } else if (showText) {
            input.classList.remove(hideTextClass);
        } else {
            input.classList.add(hideTextClass);
        }
    }
}