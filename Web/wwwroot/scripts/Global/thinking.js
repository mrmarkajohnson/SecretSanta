window.addEventListener('load', function () {
    initThinking();
});

function initThinking() {
    let thinkingSection = document.querySelector('.thinking');

    var forms = document.querySelectorAll('form');
    forms.forEach(initSubmitThinking);

    function initSubmitThinking(form) {
        form.addEventListener('submit', function() {
            thinkingSection.style.display = "block";
        });
    }
}