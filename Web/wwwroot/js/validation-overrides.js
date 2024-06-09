window.addEventListener('load', function () {
    initValidationOverrides();
});

$(document).on('ajaxComplete', function () { // this is very difficult without JQuery
    initValidationOverrides();
});

function initValidationOverrides() {
    //var forms = document.querySelectorAll('form');
    //forms.forEach(overrideValidationMessages);

    //function overrideValidationMessages(form) {
    //    overrideMessagesForAttribute('data-val-required');
    //    overrideMessagesForAttribute('data-val-maxlength');

    //    function overrideMessagesForAttribute(attribute) {
    //        let affectedInputs = form.querySelectorAll('input[' + attribute + ']');
    //        affectedInputs.forEach(function(input) {
    //            let message = input.getAttribute(attribute).replace('a string or array type with ', '');
    //            if (message) {
    //                if (message.startsWith('The field ')) {
    //                    message = message.substr(10, 999)
    //                    input.setAttribute(attribute, message);
    //                } else if (message.startsWith('The ') && message.includes(' field is ')) {
    //                    message = message.substr(4, 999).replace(' field is ', ' is ');
    //                    input.setAttribute(attribute, message);
    //                }
    //            }
    //        });
    //    }
    //}
};