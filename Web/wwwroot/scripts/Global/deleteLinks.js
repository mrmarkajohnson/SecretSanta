function initDeleteLinks() {
    let deleteLinks = document.querySelectorAll('a.delete-link');
    deleteLinks.forEach(initDeleteLink);
}

function initDeleteLink(deleteLink) {
    if (!deleteLink.getAttribute('data-initialised')) {
        deleteLink.setAttribute('data-initialised', true);

        deleteLink.addEventListener('click', function () {
            confirmAndDelete(deleteLink);
        });
    }
}

function confirmAndDelete(deleteLink) {

    let title = deleteLink.getAttribute('data-confirm-title') ?? 'Delete this item';
    let message = deleteLink.getAttribute('data-confirm-message') ?? 'Are you sure?';

    bootbox.confirm({
        title: title,
        message: message,
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-no'
            }
        },
        callback: function (result) {
            bootbox.hideAll(); // avoid issues with the bootbox not closing the second time it's opened

            if (result) {
                deleteItem(deleteLink);
            }
        }
    });

    async function deleteItem(deleteLink) {
        let deleteUrl = deleteLink.getAttribute('data-url');
        let url = new URL(deleteUrl);

        let response = await fetch(url.href,
            {
                method: "POST",
                redirect: 'follow'
            });

        await response;
        document.dispatchEvent(new Event('ajaxComplete'));
        let responseText = await response.text();

        if (response.redirected) {
            window.location.href = response.url;
        }
        else if (response.ok) {
            reloadGrid();
        }

        if (!response.redirected && responseText != null && responseText != '') {
            if (response.ok) {
                showSuccessMessage(responseText);
            } else {
                showErrorMessage(responseText);
            }
        }
    }
}