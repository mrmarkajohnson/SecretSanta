window.addEventListener('load', function () {
    initStatusSelect();
});

document.addEventListener('reloadend', function (e) {
    initStatusSelect();
});

function initStatusSelect() {
    let selectRadios = document.querySelectorAll('input.user-select');
    let selectUrl = document.querySelector('div.user-grid-container').getAttribute('data-url');
    let url = new URL(selectUrl);
    
    selectRadios.forEach(function (x) {
        if (!x.getAttribute('data-initialised')) {
            x.setAttribute('data-initialised', true);

            x.addEventListener('click', function (e) {
                let name = x.getAttribute('data-name');
                relationshipStatusChanged(e.currentTarget,
                    url,
                    'Add relationship',
                    'Are you sure you want to add a relationship with ' + name + '?');
            });
        }
    });
}
