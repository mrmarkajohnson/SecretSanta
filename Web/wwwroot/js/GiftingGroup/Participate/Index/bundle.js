function showRecipient(link) {
    let message = 'This year you are giving to '
        + link.getAttribute('data-name')
        + ' (' + link.getAttribute('data-user-name') + ').'
    bootbox.alert(message);
}