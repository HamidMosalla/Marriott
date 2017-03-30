(function () {

    var placeHoldOnCreditCardHub = $.connection.placeHoldOnCreditCardHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();

    $("#form").submit(function (event)
    {
        if ($("#PlaceHoldOnCreditCardFailed").val() !== "False")
        {
            event.preventDefault();

            var reservationId = $("#ReservationId").val();
            var creditCardNumber = $("#CreditCardNumber").val();
            var creditCardExpiryMonth = $("#CreditCardExpiryMonth").val();
            var creditCardExpiryYear = $("#CreditCardExpiryYear").val();
            var creditCardCcv = $("#CreditCardCcv").val();

            placeHoldOnCreditCardHub.server.placeHoldOnCreditCard(reservationId, creditCardNumber, creditCardExpiryMonth, creditCardExpiryYear, creditCardCcv);

            $("#mySpinner").css("display", "inline-block");
            $("#mySubmitButton").css("display", "none");
        }
    });

    placeHoldOnCreditCardHub.client.placeHoldOnCreditCardResult = function (succeeded)
    {
        $("#mySpinner").css("display", "none");

        if (!succeeded) {
            $('#errorMessage').show();
            $("#PlaceHoldOnCreditCardFailed").val = "True";
            $("#mySubmitButton").css("display", "inline-block");
        }
        else {
            $('#errorMessage').hide();
            $("#PlaceHoldOnCreditCardFailed").val("False");
            $("#form").submit();
        }
    }

}());