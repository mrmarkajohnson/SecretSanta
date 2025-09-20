function initBackgroundLinks() {
    let backgroundLinks = document.querySelectorAll('.background-link');
    backgroundLinks.forEach(initBackgroundLink);
}

function initBackgroundLink(backgroundLink) {
    if (!initialised(backgroundLink, 'background-link')) {
        backgroundLink.addEventListener('click', function () {
            confirmAndFollow(backgroundLink);
        });
    }
}

function confirmAndFollow(backgroundLink) {
    let message = backgroundLink.getAttribute('data-confirm-message');

    if (isEmptyValue(message)) {
        followLink(backgroundLink);
    }
    else {
        let title = backgroundLink.getAttribute('data-confirm-title');

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
                    followLink(backgroundLink);
                } else if (backgroundLink.tagName == 'INPUT' && (backgroundLink.type == 'checkbox' || backgroundLink.type == 'radio')) {
                    backgroundLink.checked = !backgroundLink.checked;
                }
            }
        });
    }
}

async function followLink(backgroundLink) {
    let linkUrl = backgroundLink.getAttribute('data-url');

    if (isEmptyValue(linkUrl))
        return false;

    let url = new URL(linkUrl);

    let response = await fetch(url.href,
        {
            method: "POST"
        });

    await response;
    document.dispatchEvent(new Event('ajaxComplete'));
    let responseText = await response.text();

    if (responseText != null && responseText != '') {
        if (response.ok) {
            showSuccessMessage(responseText);
        } else {
            showErrorMessage(responseText);
        }
    }

    try {
        reloadGrid();
    } catch { }
}