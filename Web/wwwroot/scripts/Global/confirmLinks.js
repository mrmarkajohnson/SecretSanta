function initConfirmLinks() {
    let confirmLinks = document.querySelectorAll('a[data-confirm-message][data-confirm-title][href]:not([href=""]):not(.background-link):not(.delete-link)');
    confirmLinks.forEach(initConfirmLink);
}

function initConfirmLink(confirmLink) {
    if (!initialised(confirmLink, 'confirm-link')) {
        confirmLink.addEventListener('click', function (e) {
            e.preventDefault();
            confirmAndFollow();
        });
    }

    function confirmAndFollow() {
        let message = confirmLink.getAttribute('data-confirm-message');

        if (isEmptyValue(message)) {
            followLink(confirmLink);
        }
        else {
            let title = confirmLink.getAttribute('data-confirm-title');

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
                        followLink(confirmLink);
                    } else if (confirmLink.tagName == 'INPUT' && (confirmLink.type == 'checkbox' || confirmLink.type == 'radio')) {
                        confirmLink.checked = !confirmLink.checked;
                    }
                }
            });
        }
    }

    function followLink() {
        window.location.href = confirmLink.href;
    }
}

