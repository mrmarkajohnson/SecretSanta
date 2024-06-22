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