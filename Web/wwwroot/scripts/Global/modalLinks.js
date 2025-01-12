function initModalLinks() {
    let modalLinks = document.querySelectorAll('a.modal-link');
    modalLinks.forEach(initModalLink);
}

function initModalLink(modalLink) {
    modalLink.addEventListener('click', function () {
        showModal(modalLink);
    });
}

async function showModal(modalLink) {
    let linkUrl = modalLink.getAttribute('data-url');
    let url = new URL(linkUrl);

    let response = await fetch(url.href,
        {
            method: "GET",
        });

    await response;
    //document.dispatchEvent(new Event('ajaxComplete'));

    let responseText = await response.text();

    if (response.ok) {
        let modalContainer = document.getElementById('modalContainer');

        if (modalContainer) {
            modalContainer.innerHTML = responseText;
        }
        else {
            document.body.insertAdjacentHTML('afterbegin', '<div id="modalContainer">' + responseText + '</div>');
            modalContainer = document.getElementById('modalContainer');
        }

        let modalContent = modalContainer.querySelector('div.modal');

        let modal = new bootstrap.Modal(modalContent);
        modal.show();
    } else {
        toastr.error(responseText);
    }
}