function initModalLinks() {
    let modalLinks = document.querySelectorAll('a.modal-link');
    modalLinks.forEach(initModalLink);
}

function initModalLink(modalLink) {
    if (!modalLink.getAttribute('data-initialised')) {
        modalLink.setAttribute('data-initialised', true);

        modalLink.addEventListener('click', function () {
            showModal(modalLink);
        });
    }
}

async function showModal(modalLink) {
    let linkUrl = modalLink.getAttribute('data-url');
    let url = new URL(linkUrl);

    let response = await fetch(url.href,
        {
            method: "GET",
        });

    await response;    

    let responseText = await response.text();

    if (response.ok) {
        showModalResponse(responseText);
        document.dispatchEvent(new Event('ajaxComplete'));
    } else {
        toastr.error(responseText);
    }
}

function showModalResponse(responseText) {
    let modalContainer = document.getElementById('modalContainer');

    if (modalContainer) {
        modalContainer.innerHTML = responseText;
    }
    else {
        document.body.insertAdjacentHTML('afterbegin', '<div id="modalContainer">' + responseText + '</div>');
        modalContainer = document.getElementById('modalContainer');
    }

    let modal = modalContainer.querySelector('div.modal');
    let modalObject = new bootstrap.Modal(modal);

    modalContainer.addEventListener('hidden.bs.modal', function () {
        if (document.activeElement) {
            document.activeElement.blur(); // avoid annoying 'Blocked aria-hidden on an element...' message
        }

        document.dispatchEvent(new CustomEvent('modalClosed', { detail: { modal: modal } }));
        document.querySelectorAll('.modal-backdrop').forEach(function (x) {
            x.remove();
        })
    });

    modalObject.show();
}
