async function relationshipStatusChanged(radio, url, title, message) {
    if (message != null && message != '') {
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
            callback: async function (result) {
                if (result) {
                    await selectUser();
                }

                radio.checked = false;
            }
        });
    }
    else {
        await selectUser();
        radio.checked = false;
    }

    async function selectUser() {
        let selectedUserId = radio.getAttribute('data-user-id');
        url.searchParams.set('userId', selectedUserId);

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
        else if (responseText != null && responseText != '') {
            if (response.ok) {
                toastr.success(responseText);
            } else {
                toastr.error(responseText);
            }
        }
    }
}