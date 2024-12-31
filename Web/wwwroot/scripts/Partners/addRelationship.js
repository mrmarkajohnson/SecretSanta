window.addEventListener('load', function () {
    initUserSelect();
});

function initUserSelect() {
    let selectRadios = document.querySelectorAll('input.user-select');
    let selectUrl = document.querySelector('div.user-grid-container').getAttribute('data-url');
    let url = new URL(selectUrl);
    
    selectRadios.forEach(function (x) {
        x.addEventListener('click', function (e) {
            userSelected(e.currentTarget, url, "Add relationship", "Are you sure you want to add a relationship with this user?");
        });
    });
}
