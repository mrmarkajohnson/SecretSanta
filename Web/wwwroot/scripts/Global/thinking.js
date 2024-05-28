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
                }, 100);
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
