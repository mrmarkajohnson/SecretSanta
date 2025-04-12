async function selectUser(radio, url) {
    let selectedUserId = radio.getAttribute('data-user-id');
    url.searchParams.set('globalUserId', selectedUserId);

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