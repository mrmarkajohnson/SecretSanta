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
    if (!initialised(container, 'input-checkbox')) {
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