document.addEventListener('modalOpened', function (e) {
    viewMessageModalClosed(e);
});

document.addEventListener('modalClosed', function (e) {
    viewMessageModalClosed(e);
});

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
