(function ($, window, document, undefined) {
   
    var loginHandler = {
        init: function () {
            function setFormValidation(id) {
                $(id).validate({
                    errorPlacement: function (error, element) {
                        $(element).parent('div').addClass('has-error');
                    }
                });
            }
            setFormValidation('#LoginValidation');
            setTimeout(function () {
                // after 1000 ms we add the class animated to the login/register card
                $('.card').removeClass('card-hidden');
            },
                700);
        }
    };
    // enable client validation for hidden fields
    $.validator.setDefaults({ ignore: [] });

    $(document).ready(function () {
        loginHandler.init();
    });
    
})(jQuery, window, document);