function CountDownTimer(timeLeftOnPendingReservation)
{
    var defaultTimer = timeLeftOnPendingReservation, callback = function ()
    {
        alert("You Reservation has Expired. Hit OK to start the Reservation process again.");
        window.location.replace("/Reservation/PendingReservationTimeOut");
    };

    //TODO: can I get rid fo the totalTime variable and just use defaultTimer directly?
    var totalTime = defaultTimer;

    var timer = setInterval(function ()
    {
        if (totalTime != -1 && !isNaN(totalTime))
        {
            var val = 'Room(s) held for: ' + (function ()
            {
                var minutes = Math.floor(totalTime / 60);
                if (minutes < 10)
                {
                    return '0'.concat(minutes);
                }
                else
                {
                    return minutes;
                }
            })() + ':' + (function ()
            {
                var seconds = totalTime % 60;
                if (seconds < 10)
                {
                    return '0'.concat(seconds);
                }
                else
                {
                    return seconds;
                }
            })();

            $('#Counter').html(val);
            $('input[name="TimeLeftOnPendingReservation"]').val(totalTime);
            totalTime--;
        }
        else
        {
            window.clearInterval(timer);
            timer = null;
            callback();
        }
    }, 1000);
};