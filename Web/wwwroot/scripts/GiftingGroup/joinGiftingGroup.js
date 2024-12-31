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

    if (!!groupNameInput && !!joinerTokenInput) {
        groupNameInput.addEventListener('change', getGroupDetails);
        joinerTokenInput.addEventListener('change', getGroupDetails);
    }

    async function getGroupDetails() {
        if (notEmptyInput(groupNameInput) && notEmptyInput(joinerTokenInput)) {
            let url = form.getAttribute('data-get-group-details');
            let data = new FormData(form);

            let response = await fetch(url,
                {
                    method: "POST",
                    body: data
                });

            form.innerHTML = await response.text();
            document.dispatchEvent(new Event('ajaxComplete'));
        } else {
            let descriptionSpan = form.querySelector('span.group-description');
            if (descriptionSpan) {
                descriptionSpan.innerHTML = '';
            }
        }
    }
}