function emptyValue(value) {
    return value== undefined || value == null || value == '';
}

function emptyInput(input) {
    return !input || emptyValue(input.value);
}

function notEmptyValue(value) {
    return !emptyValue(value);
}

function notEmptyInput(input) {
    return input && notEmptyValue(input.value);
}