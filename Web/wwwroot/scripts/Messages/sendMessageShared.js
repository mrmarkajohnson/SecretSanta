function initSendMessage() {
    initSummernote();

    let form = document.getElementById('messageForm');

    if (form && !form.getAttribute('data-initalised-wm')) {
        form.setAttribute('data-initalised-wm', true);


    }
}