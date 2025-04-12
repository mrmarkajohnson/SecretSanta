window.addEventListener('load', function () {
    initStatusSelect();
});

document.addEventListener('reloadend', function (e) {
    initStatusSelect();
});

function initStatusSelect() {
    let selectRadios = document.querySelectorAll('input.user-select');
    let selectUrl = document.querySelector('div.user-grid-container').getAttribute('data-url');
    let url = new URL(selectUrl);

    selectRadios.forEach(function (x) {
        if (!x.getAttribute('data-initialised')) {
            x.setAttribute('data-initialised', true);

            x.addEventListener('click', function (e) {
                let name = x.getAttribute('data-name');
                addRelationship(e.currentTarget,
                    url,
                    'Add relationship',
                    'Are you currently in a relationship with ' + name + '?');
            });
        }
    });
}

async function addRelationship(radio, url, title, message) {
    bootbox.dialog({
        title: title,
        message: message,
        size: 'large',
        buttons: {
            yes: {
                label: 'Yes',
                className: 'btn-success',
                callback: async function () {
                    await userSelected(true);
                    radio.checked = false;
                }
            },
            no: {
                label: 'No, but we were in a relationship',
                className: 'btn-no',
                callback: async function () {
                    await userSelected(false);
                    radio.checked = false;
                }
            },
            cancel: {
                label: 'Cancel',
                className: 'btn-default',
                callback: async function () {
                    radio.checked = false;
                }
            }
        }
    });

    function userSelected(active) {
        url.searchParams.set('active', active);
        selectUser(radio, url);
    }
}
