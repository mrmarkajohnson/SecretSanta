window.addEventListener('load', function () {
    initReviewInvitation();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initReviewInvitation();
});

function initReviewInvitation() {
    let form = document.querySelector('form.review-invitation');
    let reviewSection = form.querySelector('review-section');
    let rejectSection = form.querySelector('reject-section');

    if (reviewSection && rejectSection) {
        
        let reviewOptions = reviewSection.querySelectorAll('input[type=radio]');

        if (reviewOptions) {
            reviewOptions.forEach(function (option) {
                if (!initialised(option, 'accept')) {
                    option.addEventListener('click', function () {
                        reviewOptionSelected(option);
                    });
                }
            });
        }

        function reviewOptionSelected(option) {
            let isRejection = option.classList.contains('radio-no');
            rejectSection.classList.toggle('collapse', isRejection);
        }
    }
}