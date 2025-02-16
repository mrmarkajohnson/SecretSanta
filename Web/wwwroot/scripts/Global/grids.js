document.addEventListener('reloadstart', function (e) {
    //console.log('grid: ', e.detail.grid);
    let gridId = e.detail.grid.element.id;
    let grid = document.getElementById(gridId);

    if (!!grid && isEmptyValue(grid.innerHTML)) {
        grid.insertAdjacentHTML('afterbegin', '<i>Loading, please wait...</i>');
    }
});

document.addEventListener('click', function (e) {
    try {
        if (e.target && e.target.classList && e.target.classList.contains('grid-content-refresh')) {
            let gridElement = e.target.closest('.mvc-grid');

            if (gridElement) {
                let grid = new MvcGrid(gridElement);

                //grid.requestType = 'post'; // defaults to get
                //grid.query.set('name', 'Joe');
                grid.reload();
            }
        }
    } catch {
        console.log('Grid reload failed');
    }
});

function reloadGrid() {
    let grid = new MvcGrid(document.querySelector('.mvc-grid'));
    grid.reload();
}