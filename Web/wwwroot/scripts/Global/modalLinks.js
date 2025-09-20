function initModalLinks() {
    let modalLinks = document.querySelectorAll('.modal-link');
    modalLinks.forEach(initModalLink);
}

function initModalLink(modalLink) {
    if (!initialised(modalLink, 'modal-link')) {
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
        showErrorMessage(responseText);
    }
}

function showModalResponse(responseText) {
    let modalContainer = document.getElementById('modalContainer');

    if (modalContainer) {
        modalContainer.removeEventListener('hidden.bs.modal', modalClosed); // prevent previous versions from throwing closure events
        modalContainer.innerHTML = responseText;
    }
    else {
        document.body.insertAdjacentHTML('afterbegin', '<div id="modalContainer">' + responseText + '</div>');
        modalContainer = document.getElementById('modalContainer');
    }

    let modal = modalContainer.querySelector('div.modal');
    let modalObject = new bootstrap.Modal(modal);

    modalContainer.addEventListener('hidden.bs.modal', modalClosed);
    document.dispatchEvent(new CustomEvent('modalOpening', { detail: { modal: modal } }));

    let saveButton = modal.querySelector('.btn-save-close-modal');
    if (!!saveButton) {
        saveButton.addEventListener('click', function () {
            saveModalForm(modal, modalObject);
        });
    }

    modalObject.show();
}

function modalClosed(e) {
    let modal = e.target;

    if (modal) {
        if (document.activeElement) {
            document.activeElement.blur(); // avoid annoying 'Blocked aria-hidden on an element...' message
        }

        document.dispatchEvent(new CustomEvent('modalClosed', { detail: { modal: modal } }));
        document.querySelectorAll('.modal-backdrop').forEach(function (x) {
            x.remove();
        });
    }
}

async function saveModalForm(modal, modalObject) {
    let form = modal.querySelector('form');
    let response = await submitFormViaFetch(form);
    let responseText = await getResponseText(response);

    if (response.ok) {
        if (isHtml(responseText)) {
            let successMessage = getSuccessMessage();

            if (!isEmptyValue(successMessage)) {
                handleSuccessfulSave(successMessage);
            }
        }
        else {
            if (!isEmptyValue(responseText)) {
                handleSuccessfulSave(responseText);
            }

            modalObject.hide();
        }
    } else if (!isEmptyValue(responseText) && !isHtml(responseText)) {
        showErrorMessage(responseText);
    }

    function handleSuccessfulSave(message) {
        showSuccessMessage(message);
        modalObject.hide();
        document.dispatchEvent(new CustomEvent('modalSaved', { detail: { modal: modal } }));
    }
}
