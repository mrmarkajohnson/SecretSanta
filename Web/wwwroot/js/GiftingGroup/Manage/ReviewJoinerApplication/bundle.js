window.addEventListener('load', function () {
    initReviewApplication();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initReviewApplication();
});

function initReviewApplication() {
    let form = document.querySelector('form.review-application');
    let acceptedOptions = form.querySelectorAll('input.accepted-option');
    let acceptedSection = form.querySelector('div.accepted-section');
    let rejectedSection = form.querySelector('div.rejected-section');
    let rejected = null;

    let acceptedSectionExists = document.body.contains(acceptedSection);
    let rejectedSectionExists = document.body.contains(rejectedSection);

    if (acceptedOptions.length > 0 && (acceptedSectionExists || rejectedSectionExists)) {
        acceptedOptions.forEach(function (radioOption) {
            acceptedOptionChanged(radioOption);

            radioOption.addEventListener('click', function () {
                acceptedOptionChanged(radioOption);
            });
        });

        function acceptedOptionChanged(radioOption) {
            let checked = radioOption.checked == true ? true : null;
            let rejectedOption = !isTrueValue(radioOption.value); // don't use isTrueInput(), as it's the value we want to know about
            let newValue = checked && rejectedOption;

            if (newValue != rejected) {
                rejected = newValue;

                if (acceptedSectionExists) {
                    if (rejected) {
                        acceptedSection.classList.add('collapse');
                    } else {
                        acceptedSection.classList.remove('collapse');
                    }
                }

                if (rejectedSectionExists) {
                    if (rejected) {
                        rejectedSection.classList.remove('collapse');
                    } else {
                        rejectedSection.classList.add('collapse');
                    }
                }
            }
        }
    }
}