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
    if (emptyValue(defaultPlaceholder)) {
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
        if (!emptyInput(dataListInput)) {
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
let isFirefox = navigator.userAgent.toLowerCase().includes('firefox');
let isEdge = navigator.userAgent.toLowerCase().includes('edge');

window.addEventListener('load', function () {
    initPopper();
    initSuccessMessage();
    initEyeSymbols();
    initDataLists()
    initThinking();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initPopper();
    initEyeSymbols();
    initDataLists()
    initThinking();
});
function initPopper() {
    $('[data-toggle="popover"]').popover();
    $('[data-toggle="tooltip"]').tooltip();
}
function initSuccessMessage() {
    let successMessageInput = document.getElementById('redirectSuccessMessage');
    if (notEmptyInput(successMessageInput)) {
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
            "timeOut": 1000,
            "extendedTimeOut": 0,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        toastr["success"](successMessageInput.value)
    }
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

async function relationshipStatusChanged(radio, url, title, message) {
    if (message != null && message != '') {
        bootbox.confirm({
            title: title,
            message: message,
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: async function (result) {
                if (result) {
                    await selectUser();
                }

                radio.checked = false;
            }
        });
    }
    else {
        await selectUser();
        radio.checked = false;
    }

    async function selectUser() {
        let selectedUserId = radio.getAttribute('data-id');
        url.searchParams.set('userId', selectedUserId);

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
                toastr.success(responseText);
            } else {
                toastr.error(responseText);
            }
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
                if (!emptyInput(input) && !showText) {
                    eyeSymbol.classList.remove('collapse');
                    if (noEyeSymbol) {
                        noEyeSymbol.classList.add('collapse');
                    }
                }                
            });

            function toggleEyeSymbol() {
                if (emptyInput(input)) {
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