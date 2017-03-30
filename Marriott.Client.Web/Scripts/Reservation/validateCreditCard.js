(function () {

    var validateCreditCardHub = $.connection.validateCreditCardHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();

    $("#form").submit(function (event)
    {
        //https://api.jquery.com/submit/
        if ($("#CreditCardCheckFailed").val() !== "False")
        {
            event.preventDefault();

            var creditCardNumberAsInt = parseInt($("#CreditCardNumber").val());
            var creditCardExpiryMonthAsInt = parseInt($("#CreditCardExpiryMonth").val());
            var creditCardExpiryYearAsInt = parseInt($("#CreditCardExpiryYear").val());
            var creditCardCcvAsInt = parseInt($("#CreditCardCcv").val());

            validateCreditCardHub.server.validate(creditCardNumberAsInt, creditCardExpiryMonthAsInt, creditCardExpiryYearAsInt, creditCardCcvAsInt);

            $("#mySpinner").css("display", "inline-block");
            $("#mySubmitButton").css("display", "none");
        }
    });

    validateCreditCardHub.client.validateCreditCardResult = function (succeeded)
    {
        $("#mySpinner").css("display", "none");

        if (!succeeded)
        {
            $('#errorMessage').show();
            $("#CreditCardCheckFailed").val = "True";
            $("#mySubmitButton").css("display", "inline-block");
        }
        else
        {
            $('#errorMessage').hide();
            $("#CreditCardCheckFailed").val("False");
            $("#form").submit(); 
        }
    }

}());