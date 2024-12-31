async function userSelected(radio, url, title, message) {
    // TODO: use bootbox.confirm to check if the person actually wants to add this relationship

    let selectedUserId = radio.getAttribute('data-id');
    url.searchParams.set('userId', selectedUserId);

    let response = await fetch(url.href,
        {
            method: "POST",
            redirect: 'follow'
        });

    await response;
    document.dispatchEvent(new Event('ajaxComplete'));
    let responseText = await response.text();

    if (responseText != null && responseText != '') {
        if (response.ok) {
            toastr.success(responseText);
        } else {
            toastr.error(responseText);
        }
    }
}