document.addEventListener('modalOpening', function (e) {
    relationshipModalOpening(e);
})

document.addEventListener('modalSaved', function (e) {
    relationshipModalSaved(e);
})
async function relationshipModalOpening(e) {
    let modal = e.detail.modal;
    if (modal.id == 'manageRelationshipModal') {
        // TODO: Complete this to show or hide the sections dynamically
    }
}

async function relationshipModalSaved(e) {
    let modal = e.detail.modal;
    if (modal.id == 'manageRelationshipModal') {
        reloadGrid();
    }
}