document.addEventListener('modalOpening', function (e) {
    relationshipModalOpening(e);
});

document.addEventListener('modalSaved', function (e) {
    relationshipModalSaved(e);
});

async function relationshipModalOpening(e) {
    let modal = e.detail.modal;
    if (modal.id == 'manageRelationshipModal') {
        relationshipModalOpened(modal);
    }
}

function relationshipModalOpened(modal) {
    let nowOptionsSection = modal.querySelector('.now-options-section');

    if (nowOptionsSection) {
        manageNowOptions(modal, nowOptionsSection);
    }
}

function manageNowOptions(modal, nowOptionsSection) {

    let everOptionsSection = modal.querySelector('.ever-options-section');
    let exchangeOptionsSection = modal.querySelector('.exchange-options-section');

    let nowNoOption = nowOptionsSection.querySelector('input[type=radio].radio-no');
    let nowYesOption = nowOptionsSection.querySelector('input[type=radio].radio-yes');
    let nowNotSureOption = nowOptionsSection.querySelector('input[type=radio].radio-not-sure');

    let currentNowOptionYes = nowYesOption.checked;

    if (everOptionsSection) {
        hideSection(everOptionsSection);

        let everNoOption = everOptionsSection.querySelector('input[type=radio].radio-no');
        let everYesOption = everOptionsSection.querySelector('input[type=radio].radio-yes');
        let everNotSureOption = everOptionsSection.querySelector('input[type=radio].radio-not-sure');

        if (exchangeOptionsSection) {
            let exchangeNoOption = exchangeOptionsSection.querySelector('input[type=radio].radio-no');
            let exchangeYesOption = exchangeOptionsSection.querySelector('input[type=radio].radio-yes');
            let exchangeNotSureOption = exchangeOptionsSection.querySelector('input[type=radio].radio-not-sure');

            nowNoOption.addEventListener('click', function() {
                nowNoOrNotSure();
            });

            nowYesOption.addEventListener('click', function() {
                hideSection(everOptionsSection);
                hideSection(exchangeOptionsSection);

                everNoOption.checked = true;
                exchangeNoOption.checked = true;

                currentNowOptionYes = true;
            });

            nowNotSureOption.addEventListener('click', function() {
                nowNoOrNotSure();
            });

            function nowNoOrNotSure() {
                showSection(everOptionsSection);
                showSection(exchangeOptionsSection);

                if (currentNowOptionYes) {
                    resetEverOptions();
                    resetExchangeOptions();
                }

                currentNowOptionYes = false;
            }

            function resetExchangeOptions() {
                exchangeNoOption.checked = exchangeYesOption.checked = exchangeNotSureOption.checked = false;
            }
        } else {
            nowNoOption.addEventListener('click', function() {
                showSection(everOptionsSection);

                if (currentNowOptionYes) {
                    resetEverOptions();
                }

                currentNowOptionYes = false;
            });
            nowYesOption.addEventListener('click', function() {
                hideSection(everOptionsSection);
                everYesOption.checked = true;

                currentNowOptionYes = true;
            });
            nowNotSureOption.addEventListener('click', function() {
                showSection(everOptionsSection);

                if (currentNowOptionYes) {
                    resetEverOptions();
                }

                currentNowOptionYes = false;
            });
        }

        function resetEverOptions() {
            everNoOption.checked = everYesOption.checked = everNotSureOption.checked = false;
        }
    }
    else if (exchangeOptionsSection) {
        let exchangeNoOption = exchangeOptionsSection.querySelector('input[type=radio].radio-no');
        let exchangeYesOption = exchangeOptionsSection.querySelector('input[type=radio].radio-yes');
        let exchangeNotSureOption = exchangeOptionsSection.querySelector('input[type=radio].radio-not-sure');

        nowNoOption.addEventListener('click', function() {
            showSection(exchangeOptionsSection);

            if (currentNowOptionYes) {
                resetExchangeOptions();
            }

            currentNowOptionYes = false;
        });

        nowYesOption.addEventListener('click', function() {
            hideSection(exchangeOptionsSection);
            exchangeNoOption.checked = true;

            currentNowOptionYes = true;
        });

        nowNotSureOption.addEventListener('click', function() {
            showSection(exchangeOptionsSection);

            if (currentNowOptionYes) {
                resetExchangeOptions();
            }

            currentNowOptionYes = false;
        });

        function resetExchangeOptions() {
            exchangeNoOption.checked = exchangeYesOption.checked = exchangeNotSureOption.checked = false;
        }
    }
}

function showSection(section) {
    if (section.classList.contains('collapse')) {
        section.classList.remove('collapse');
    }
}

function hideSection(section) {
    if (!section.classList.contains('collapse')) {
        section.classList.add('collapse');
    }
}

async function relationshipModalSaved(e) {
    let modal = e.detail.modal;
    if (modal.id == 'manageRelationshipModal') {
        reloadGrid();
    }
}