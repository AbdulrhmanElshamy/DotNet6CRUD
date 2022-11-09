//toaster options
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-center",
    "preventDuplicates": true,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}


$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);
        if (confirm("Are you shur you need delete it")) {
            $.post({
                url: btn.data('url'),
                success: function () {
                    btn.parents('.col-12').fadeOut()
                    toastr.success("Done")
                }
            })
        }
    })
})
