document.addEventListener('modalClosed', function (e) {
    messageModalClosed(e);
})

async function messageModalClosed(e) {
    let modal = e.detail.modal;
    if (modal.id == 'viewMessageModal') {
        let read = modal.getAttribute('data-read');
        if (isTrueValue(read))
            return true;

        let closeUrl = modal.getAttribute('data-close-url');
        let id = modal.getAttribute('data-id');

        if (!isEmptyValue(closeUrl) && !isEmptyValue(id)) {
            let url = new URL(closeUrl);
            url.searchParams.set('id', id);

            let response = await fetch(url.href,
                {
                    method: "POST"
                });

            await response;
            reloadGrid();
        }
    }
}
