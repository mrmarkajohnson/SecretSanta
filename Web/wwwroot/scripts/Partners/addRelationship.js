window.addEventListener('load', function () {
    initStatusSelect();
});

function initStatusSelect() {
    let selectRadios = document.querySelectorAll('input.user-select');
    let selectUrl = document.querySelector('div.user-grid-container').getAttribute('data-url');
    let url = new URL(selectUrl);
    
    selectRadios.forEach(function (x) {
        x.addEventListener('click', function (e) {
            let name = x.getAttribute('data-name');
            relationshipStatusChanged(e.currentTarget,
                url,
                'Add relationship',
                'Are you sure you want to add a relationship with ' + name + '?');
        });
    });
}
