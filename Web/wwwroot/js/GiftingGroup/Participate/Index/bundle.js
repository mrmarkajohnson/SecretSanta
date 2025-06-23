function showRecipient(link) {
    let title = '\'' + link.getAttribute('data-group-name') + '\' Recipient';
    let message = 'This year you are giving to ' + link.getAttribute('data-user-name') + '.';

    let infoCard = link.parentElement.querySelector('span.user-info-card');

    if (infoCard) {
        message += infoCard.innerHTML;
    }

    bootbox.alert({ title: title, message: message });
    setTooltips();
}