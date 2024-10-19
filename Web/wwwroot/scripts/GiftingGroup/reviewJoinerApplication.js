window.addEventListener('load', function () {
    initReviewApplication();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initReviewApplication();
});

function initReviewApplication() {
    let form = document.querySelector('form.review-application');
    let acceptedOptions = form.querySelectorAll('input.accepted-option');
    let rejectedSection = form.querySelector('div.rejected-section');
    let rejected = false;

    if (acceptedOptions.length > 0 && document.body.contains(rejectedSection)) {
        acceptedOptions.forEach(function (radioOption) {
            acceptedOptionChanged(radioOption);

            radioOption.addEventListener('click', function () {
                acceptedOptionChanged(radioOption);
            });
        });
    }

    function acceptedOptionChanged(radioOption) {
        let checked = !!radioOption.checked;
        let rejectedOption = !!radioOption.value == false || radioOption.value == 'false';
        let newValue = checked && rejectedOption;

        if (newValue != rejected) {
            rejected = newValue;

            if (rejected) {
                rejectedSection.classList.remove('collapse');
            } else {
                rejectedSection.classList.add('collapse');
            }
        }
    }
}