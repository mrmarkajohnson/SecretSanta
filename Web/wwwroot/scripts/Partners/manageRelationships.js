window.addEventListener('load', function () {
    initStatusSelects();
    initEditLinks();
    initDeleteLinks();
});

function initStatusSelects() {
    let statusSelects = document.querySelectorAll('select.relationship-status-select');
    let selectUrl = document.querySelector('div.relationships-table').getAttribute('data-change-url');
    let url = new URL(selectUrl);

    statusSelects.forEach(function (x) {
        x.setAttribute('data-original-value', x.value);

        if (!x.getAttribute('data-initialised')) {
            x.setAttribute('data-initialised', true);

            x.addEventListener('change', function (e) {
                let originalValue = x.getAttribute('data-original-value');

                if (x.value != originalValue) {
                    let name = x.getAttribute('data-name');
                    let headerText = 'Change relationship status';
                    let messageText = 'Are you sure you want to change the status of your relationship with ' + name + '?';

                    if (x.value == 'NotRelationship') { // TODO: More specific wording for other values
                        headerText = 'Deny suggesteed relationship'
                        messageText = 'You\'re NOT in a relationship with ' + name + ', is that correct?';
                    }

                    relationshipStatusChanged(e.currentTarget,
                        url,
                        headerText,
                        messageText);
                }
            });
        }
    });
}

function initEditLinks() {
    let editLinks = document.querySelectorAll('a.edit-relationship-link');
    let editUrl = document.querySelector('div.relationships-table').getAttribute('data-edit-url');
    let url = new URL(editUrl);

    editLinks.forEach(function (x) {
        if (!x.getAttribute('data-initialised')) {
            x.setAttribute('data-initialised', true);

            x.addEventListener('click', function (e) {
                
            });
        }
    });
}

function initDeleteLinks() {
    let deleteLinks = document.querySelectorAll('a.delete-relationship-link');
    let deleteUrl = document.querySelector('div.relationships-table').getAttribute('data-delete-url');
    let url = new URL(deleteUrl);

    deleteLinks.forEach(function (x) {
        if (!x.getAttribute('data-initialised')) {
            x.setAttribute('data-initialised', true);
            x.setAttribute('data-original-value', 'null'); // avoid breaking the relationshipStatusChanged method

            x.addEventListener('click', function (e) {
                let name = x.getAttribute('data-name');
                let headerText = 'Delete relationship';
                let messageText = 'Are you sure you want to delete your proposed relationship with ' + name + '?';

                relationshipStatusChanged(e.currentTarget,
                    url,
                    headerText,
                    messageText);
            });
        }
    });
}

async function relationshipStatusChanged(control, url, title, message) {
    let originalValue = control.getAttribute('data-original-value');

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
                statusChanged();
            }
            else {
                control.value = originalValue;
            }
        }
    });

    async function statusChanged() {
        let partnerLinkKey = control.getAttribute('data-link-id');
        let globalUserId = control.getAttribute('data-user-id');

        url.searchParams.set('partnerLinkKey', partnerLinkKey);
        url.searchParams.set('globalUserId', globalUserId);
        url.searchParams.set('newStatus', control.value);

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
            control.setAttribute('data-original-value', control.value);
            reloadGrid();
        } else {
            control.value = originalValue;
        }

        if (!response.redirected && responseText != null && responseText != '') {
            if (response.ok) {
                toastr.success(responseText);
            } else {
                toastr.error(responseText);
            }
        }
    }
}