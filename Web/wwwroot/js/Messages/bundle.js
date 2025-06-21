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
        let messageRecipientKey = modal.getAttribute('data-message-recipient-key');

        if (!isEmptyValue(closeUrl) && !isEmptyValue(messageRecipientKey)) {
            let url = new URL(closeUrl);
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


    }
}