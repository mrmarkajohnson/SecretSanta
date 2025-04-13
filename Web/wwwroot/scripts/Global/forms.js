async function submitFormViaFetch(form, url) {
    if (isEmptyValue(url)) {
        url = form.getAttribute('action');
    }

    let data = new FormData(form);

    let response = await fetch(url,
        {
            method: "POST",
            body: data,
            redirect: 'follow'
        });

    let responseClone = response.clone(); // this allows us to use response.text() on the return result; it can only be called once

    if (response.status > 400) {
        showErrorMessage('Unknown error, please contact an administrator');
    }
    else if (response.redirected) {
        window.location.href = response.url;
    }
    else {
        let responseText = await getResponseText(response);

        if (isHtml(responseText)) {
            if (responseText.includes('<form')) {
                let parser = new DOMParser();
                let responseHtml = parser.parseFromString(responseText, "text/html");
                let responseForm = responseHtml.querySelector('form');
                form.innerHTML = responseForm.innerHTML;
            }
            else {
                form.innerHTML = responseText;
            }
        }
    }

    document.dispatchEvent(new Event('ajaxComplete'));
    return responseClone;
}
