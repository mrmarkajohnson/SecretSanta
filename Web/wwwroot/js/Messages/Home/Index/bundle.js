document.addEventListener('modalClosed', function (e) {
    messageModalClosed(e);
})

async function messageModalClosed(e) {
    let modal = e.detail.modal;
    if (modal.id == 'viewMessageModal') {
        let closeUrl = modal.getAttribute('data-close-url');
        let id = modal.getAttribute('data-id');

        if (!isEmptyString(closeUrl) && !isEmptyString(id)) {
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
