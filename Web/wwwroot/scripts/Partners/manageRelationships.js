window.addEventListener('load', function () {
    initStatusSelect();
});

function initStatusSelect() {
    let statusSelects = document.querySelectorAll('select.relationship-status-select');
    let selectUrl = document.querySelector('div.relationships-table').getAttribute('data-url');
    let url = new URL(selectUrl);

    statusSelects.forEach(function (x) {
        x.setAttribute('data-original-value', x.value);
        x.addEventListener('click', function (e) {
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
    });
}

async function relationshipStatusChanged(select, url, title, message) {
    let originalValue = select.getAttribute('data-original-value');

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
                select.value = originalValue;
            }
        }
    });

    async function statusChanged() {
        let partnerLinkId = select.getAttribute('data-link-id');
        let userId = select.getAttribute('data-user-id');

        url.searchParams.set('partnerLinkId', partnerLinkId);
        url.searchParams.set('userId', userId);
        url.searchParams.set('newStatus', select.value);

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
            select.setAttribute('data-original-value', select.value);
            reloadGrid();
        } else {
            select.value = originalValue;
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
