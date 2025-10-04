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
