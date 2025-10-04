window.addEventListener('pageshow', function () {
    let messageKey = document.getElementById('messageKey').value;
    let recipientKey = document.getElementById('messageRecipientKey').value;

    if (messageKey > 0) {
        let viewMessageSelector = '.view-message-link[data-message-key="' + messageKey + '"]';
        let recipientSelector = viewMessageSelector + '[data-message-recipient-key="' + recipientKey + '"]';

        let viewLink = document.querySelector(recipientSelector);

        if (!viewLink)
            viewLink = document.querySelector(viewMessageSelector);

        if (viewLink) {
            viewLink.dispatchEvent(new Event('click'));
        }
    }
});

document.addEventListener('modalOpening', function (e) {
    writeMessageModalOpening(e);
});

document.addEventListener('modalClosed', function (e) {
    viewMessageModalClosed(e);
});

async function writeMessageModalOpening(e) {
    let modal = e.detail.modal;
    if (modal.id == 'writeMessageModal') {
        initSendMessage();
    }
}

async function viewMessageModalClosed(e) {
    let modal = e.detail.modal;
    if (modal.id == 'viewMessageModal') {
        let read = modal.getAttribute('data-read');
        if (isTrueValue(read))
            return true;

        let closeUrl = modal.getAttribute('data-close-url');
        let messageKey = modal.getAttribute('data-message-key');
        let messageRecipientKey = modal.getAttribute('data-message-recipient-key');

        if (!isEmptyValue(closeUrl) && !isEmptyValue(messageRecipientKey)) {
            let url = new URL(closeUrl);
            url.searchParams.set('messageKey', messageKey);
            url.searchParams.set('messageRecipientKey', messageRecipientKey);

            let response = await fetch(url.href,
                {
                    method: "POST"
                });

            await response;
            reloadGrid();
        }
    }
}

window.addEventListener('load', function () {
    initSendMessage();
});

document.addEventListener('reloadend', function (e) {
    initSendMessage();
});
function initSendMessage() {
    initSummernote();

    let form = document.getElementById('messageForm');

    if (form && !form.getAttribute('data-initalised-wm')) {
        form.setAttribute('data-initalised-wm', true);

        let groupSelect = form.querySelector('select.gifting-group-select');
        let recipientSection = form.querySelector('.message-recipient-section');

        if (groupSelect && recipientSection && !initialised(groupSelect, 'recipient')) {
            groupSelect.addEventListener('change', function () {
                groupChanged(groupSelect, recipientSection);
            });
        }

        initRecipientSection(form);
    }

    async function groupChanged(groupSelect, recipientSection) {
        if (isEmptyInput(groupSelect)) {
            groupEmpty(recipientSection);
        } else {
            await groupSet(groupSelect, recipientSection);
        }
    }

    function groupEmpty(recipientSection) {
        let recipientTypeSelect = recipientSection.querySelector('select.recipient-type-select');
        recipientTypeSelect.value = 'TBC';
        recipientTypeSelect.setAttribute('disabled', true);

        let recipientTypeExplanation = recipientTypeSelect.parentElement.querySelector('div.input-group-text');
        recipientTypeExplanation.setAttribute('data-bs-original-title', 'Please select a group first.')
        setTooltips();
    }

    async function groupSet(groupSelect, recipientSection) {
        let sectionUrl = groupSelect.getAttribute('data-url');
        let url = new URL(sectionUrl);

        let data = new FormData(form);

        let response = await fetch(url.href,
            {
                method: "POST",
                body: data
            });

        await response;
        let responseText = await getResponseText(response);

        if (isHtml(responseText)) {
            recipientSection.innerHTML = responseText;
            initRecipientSection(form);
        }
    }
}

function initRecipientSection(form) {
    let recipientTypeSelect = form.querySelector('select.recipient-type-select');

    if (recipientTypeSelect) {
        let futureMembersSection = form.querySelector('.future-members-section');
        let futureMembersLabel = futureMembersSection.querySelector('label.include-future-members-label');
        let futureMembersExplanation = futureMembersSection.querySelector('div.d-inline');

        let specificRecipientSection = form.querySelector('.specific-recipient-section');

        recipientTypeChanged();

        recipientTypeSelect.addEventListener('change', function() {
            recipientTypeChanged();
        });

        function recipientTypeChanged() {
            let selectedOption = recipientTypeSelect.options[recipientTypeSelect.selectedIndex];
            let specificMember = selectedOption.getAttribute('data-specific');

            if (isTrueValue(specificMember)) {
                futureMembersSection.classList.add('collapse');
                specificRecipientSection.classList.remove('collapse');
            }
            else {
                toggleFutureMembers();
            }

            function toggleFutureMembers() {
                specificRecipientSection.classList.add('collapse');
                specificRecipientSection.querySelector('select').value = '';

                let futureLabel = selectedOption.getAttribute('data-future-label');
                let futureExplanation = selectedOption.getAttribute('data-future-explanation');

                if (isEmptyValue(futureLabel)) {
                    futureMembersSection.classList.add('collapse');
                    futureMembersLabel.innerHTML = 'Include Future Members'; // in case something fails later
                } else {
                    futureMembersSection.classList.remove('collapse');
                    futureMembersLabel.innerHTML = futureLabel;
                }

                if (isEmptyValue(futureExplanation)) {
                    futureMembersExplanation.setAttribute('data-bs-original-title', ''); // in case something fails later
                } else {
                    futureMembersExplanation.setAttribute('data-bs-original-title', futureExplanation); // just in case something fails later
                }

                setTooltips();
            }
        }
    }
}
