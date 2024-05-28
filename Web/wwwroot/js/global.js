$(function () {
    initPopper();
})

$(document).on('ajaxComplete', function () {
    initPopper();
});

function initPopper() {
    $('[data-toggle="popover"]').popover();
    $('[data-toggle="tooltip"]').tooltip();
}
let submitTimer = null;

window.addEventListener('load', function () {
    initThinking();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initThinking();
});

function initThinking() {
    let thinkingSection = document.querySelector('.thinking');
    clearThinkingElements(thinkingSection, null); 

    var forms = document.querySelectorAll('form');
    forms.forEach(initSubmitThinking);

    function initSubmitThinking(form) {
        let $form = $(form); // .valid() is a JQuery extension
        let submitButton = null;        

        form.addEventListener('submit', function (e) {
            
            if ($form.valid()) {
                submitButton = e.currentTarget;
                submitButton.setAttribute('disabled', 'disabled');
                submitTimer = setTimeout(function () {
                    thinkingSection.style.display = "flex";
                    submitButton.removeAttribute('disabled');
                }, 500);
            }
        }, { passive: true });

        $('form').on('invalid-form.validate', function () { // need JQuery to get this
            clearThinkingElements(thinkingSection, submitButton);            
        });
    }
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

window.addEventListener('load', function () {
    initEyeSymbols();
});

function initEyeSymbols() {
    let eyeSymbols = document.querySelectorAll('.fa-eye');
    eyeSymbols.forEach(initEyeSymbol);
}

function initEyeSymbol(eyeSymbol) {
    let inputGroup = eyeSymbol.closest('.input-group');
    if (inputGroup) {
        let input = inputGroup.querySelector('input[type=password]');
        if (input) {
            let showText = false;
            let noEyeSymbol = inputGroup.querySelector('.fa-eye-slash');;

            let eventList = ["change", "keyup", "paste", "input"];
            for (e of eventList) {
                input.addEventListener(e, toggleEyeSymbol);
            }

            addToggleListener(eyeSymbol);

            input.addEventListener('blur', function () {
                input.type = 'password';
                showText = false;
                eyeSymbol.classList.add('collapse');
                if (noEyeSymbol) {
                    noEyeSymbol.classList.add('collapse');
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
                input.type = 'password';

                eyeSymbol.classList.remove('collapse');
                if (noEyeSymbol) {
                    noEyeSymbol.classList.add('collapse');
                }
            }

            function convertToText() {
                showText = true;
                input.type = 'text';
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
}