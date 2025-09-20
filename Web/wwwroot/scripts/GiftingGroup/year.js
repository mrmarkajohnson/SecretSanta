window.addEventListener('load', function () {
    initIncludedRadios();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initIncludedRadios();
});

function initIncludedRadios() {
    let includedRadios = document.querySelectorAll('input.included-radio');

    includedRadios.forEach(function (radio) {
        radio.setAttribute('data-original-value', radio.checked);

        if (!initialised(radio, 'participate-option')) {

            radio.addEventListener('click', function () {
                let included = isTrueValue(radio.value);
                let title = included ? 'Confirm participation' : 'Don\'t participate';
                let message = 'Are you sure you ' + (included ? '' : 'DON\'T ') + 'want to participate this year?'

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
                            className: 'btn-no'
                        }
                    },
                    callback: function (result) {
                        bootbox.hideAll(); // avoid issues with the bootbox not closing the second time it's opened
                        if (result) {
                            statusChanged(radio);
                        } else {
                            resetIncludedRadios();
                        }
                    }
                });
            });
        }
    });

    async function statusChanged(radio) {
        let div = radio.closest('div');
        let form = div.closest('form');

        let divUrl = div.getAttribute('data-url');
        let url = new URL(divUrl);
        let data = new FormData(form);

        let response = await fetch(url.href,
            {
                method: "POST",
                body: data,
                redirect: 'follow'
            });

        await response;
        document.dispatchEvent(new Event('ajaxComplete'));

        if (response.redirected) {
            window.location.href = response.url;
        }
        else {
            let responseText = await response.text();
            if (responseText == null || responseText == '') {
                responseText = response.ok ? 'Preference set successfully' : 'Could not set preference';
            }

            if (response.ok) {
                showSuccessMessage(responseText);
                includedRadios.forEach(function (x) {
                    x.setAttribute('data-original-value', x.checked);
                });
            } else {
                showErrorMessage(responseText);                
                resetIncludedRadios();
            }
        }
    }

    function resetIncludedRadios() {
        includedRadios.forEach(function(x) {
            try {
                x.checked = isTrueValue(x.getAttribute('data-original-value'));
            } catch {
                x.checked = !x.checked;
            }
        });
    }
}
