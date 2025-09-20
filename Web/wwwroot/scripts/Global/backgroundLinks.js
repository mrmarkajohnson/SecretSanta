function initBackgroundLinks() {
    let backgroundLinks = document.querySelectorAll('a.background-link');
    backgroundLinks.forEach(initBackgroundLink);
}

function initBackgroundLink(backgroundLink) {
    if (!initialised(backgroundLink, 'background-link')) {
        backgroundLink.addEventListener('click', function () {
            followLink(backgroundLink);
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
}