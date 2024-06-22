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
