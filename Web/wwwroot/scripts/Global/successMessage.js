function initSuccessMessage() {
    let successMessageInput = document.getElementById('redirectSuccessMessage');
    if (notEmptyInput(successMessageInput)) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": 1000,
            "hideDuration": 1000,
            "timeOut": 1000,
            "extendedTimeOut": 0,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        toastr["success"](successMessageInput.value)
    }
}