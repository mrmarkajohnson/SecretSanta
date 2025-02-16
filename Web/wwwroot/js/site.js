function isEmptyValue(value) {
    try {
        return value == undefined || value == null || value == '';
    } catch {
        return false;
    }
}

function isEmptyInput(input) {
    try {
        return !input || isEmptyValue(input.value);
    } catch {
        return false;
    }
}

function notEmptyValue(value) {
    return !isEmptyValue(value);
}

function notEmptyInput(input) {
    return input && notEmptyValue(input.value);
}

/**
 * Checks whether a value is effectively true or false (but not 'truthy' or 'falsy')
 * Don't use this for checkboxes or radio buttons, use their 'checked' property instead, or use isTrueInput() for any input
 */
function isTrueValue(value) {
    if (isEmptyValue(value)) return false;
    if (value === true || value == 'True' || value == 'true') return true;
    if (value === false || value == 'False' || value == 'false') return false;
    if (value === 1 || value == '1') return true;
    if (value === 0 || value == '0') return false;
    return !!value;
}

/**
 * Checks whether the input is effectively true or false (but not 'truthy' or 'falsy')
 * For checkboxes or radio buttons, this returns whether they're checked, otherwise it returns the value
 */
function isTrueInput(input) {
    try {
        if (isEmptyInput(input)) return false;
        if (input.tagName == 'INPUT' && (input.type == 'checkbox' || input.type == 'radio')) return input.checked;
        return isTrueValue(input.value);
    } catch {
        return false;
    }
}