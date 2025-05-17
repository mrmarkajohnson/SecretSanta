window.addEventListener('load', function () {
    initNameVariations();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initNameVariations();
});

function initNameVariations() {
    let showNameContainer = document.querySelector('div.name-variations-container');
    if (showNameContainer) {
        let form = showNameContainer.closest('form');
        if (form) {
            let containerUrl = showNameContainer.getAttribute('data-url');
            if (!isEmptyValue(containerUrl)) {
                let url = new URL(containerUrl);
                let inputs = document.querySelectorAll('input.show-name');
                inputs.forEach(initNameVariation);

                function initNameVariation(input) {
                    if (!input.getAttribute('data-initialised-snv')) {
                        input.setAttribute('data-initialised-snv', true);
                        input.addEventListener('change', showNameVariation);
                    }
                }

                async function showNameVariation() {
                    let data = new FormData(form);

                    let response = await fetch(url.href,
                        {
                            method: "POST",
                            body: data
                        });

                    await response;
                    let responseText = await getResponseText(response);

                    if (isHtml(responseText)) {
                        showNameContainer.innerHTML = responseText;
                    } else {
                        showNameContainer.innerHTML = '';
                    }
                }
            }
        }
    }
}