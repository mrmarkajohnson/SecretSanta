document.addEventListener('reloadstart', function (e) {
    //console.log('grid: ', e.detail.grid);
    let gridId = e.detail.grid.element.id;
    document.getElementById(gridId).insertAdjacentHTML('afterbegin', '<i>Loading, please wait...</i>');
});

function reloadGrid() {
    let grid = new MvcGrid(document.querySelector('.mvc-grid'));
    grid.reload();
}