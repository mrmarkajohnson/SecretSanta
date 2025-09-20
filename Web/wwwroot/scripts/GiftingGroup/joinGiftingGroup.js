window.addEventListener('load', function () {
    initJoinGiftingGroup();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initJoinGiftingGroup();
});

function initJoinGiftingGroup() {
    let form = document.querySelector('form.join-group-form');
    let groupNameInput = form.querySelector('input.gifting-group-name');
    let joinerTokenInput = form.querySelector('input.joiner-token');

    if (!!groupNameInput && !initialised(groupNameInput, 'join-group') && !!joinerTokenInput && !initialised(joinerTokenInput, 'join-group')) {
        groupNameInput.addEventListener('change', getGroupDetails);
        joinerTokenInput.addEventListener('change', getGroupDetails);
    }

    async function getGroupDetails() {
        if (notEmptyInput(groupNameInput) && notEmptyInput(joinerTokenInput)) {
            let url = form.getAttribute('data-get-group-details');
            await submitFormViaFetch(form, url);
        } else {
            let descriptionSpan = form.querySelector('span.group-description');
            if (descriptionSpan) {
                descriptionSpan.innerHTML = '';
            }
        }
    }
}
