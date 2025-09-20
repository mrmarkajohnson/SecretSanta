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
