using System;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Marriott.Business.Guest;
using Marriott.Business.Guest.Data;
using Marriott.Business.Reservation;
using Marriott.Business.Reservation.Data;
using Marriott.ITOps.Notifications.Email.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.Notifications.Email.Generate.Handlers
{
    public class SendReservationConfirmationEmailHandler : IHandleMessages<SendReservationConfirmationEmail>
    {
        public async Task Handle(SendReservationConfirmationEmail message, IMessageHandlerContext context)
        {
            ConfirmedReservation reservation;
            using (var reservationContext = new ReservationContext())
            {
                reservation = await reservationContext.ConfirmedReservations.SingleAsync(x => x.ReservationId == message.ReservationId);
            }

            GuestInformation guestInformation;
            using (var guestContext = new GuestContext())
            {
                guestInformation = await guestContext.GuestInformation.SingleAsync(x => x.ReservationId == message.ReservationId);
            }

            //for plain text email the AppendLine, line is not putting in a space... might need to use Environment.NewLine?
                var body = new StringBuilder();
            body.AppendLine($"Dear {guestInformation.FirstName},");
            body.AppendLine("This message is to remind you that you have made a reservation with Marriott HOtels. Your reservation information is summarized below.");
            body.AppendLine("Thank you for choosing Marriott");
            body.AppendLine(Environment.NewLine);
            body.AppendLine("-----------------------");
            body.AppendLine("RESERVATION INFORMATION");
            body.AppendLine("-----------------------");
            body.AppendLine($"Confirmation Number: {reservation.ExternalId}");
            body.AppendLine($"Name: {guestInformation.FullName}");
            body.AppendLine($"Check In: {reservation.CheckIn.ToShortDateString()}");
            body.AppendLine($"Check Out: {reservation.CheckOut.ToShortDateString()}");
            body.AppendLine(Environment.NewLine);
            body.AppendLine("------------------------------------");
            body.AppendLine("TO MODIFY OR CANCEL THIS RESERVATION");
            body.AppendLine("------------------------------------");
            body.AppendLine("Please click the link below to modify or cancel this reservation.");

            //TODO: build a UI the guest can come to in order to see their reservation details. They should be able to look it up by reservation id and/or email.
            //body.AppendLine("https://www.enterprise.com/car_rental/deeplinkmap.do?bid=001&confirmnum=1107992614&firstname=MICHAEL&lastname=MCCARTHY&cnty=US&language=EN");
            //make sure when showing payment info, it's scrubbed, so there is no security thread

            await context.Send(new SendEmail
            {
                Email = new ITOps.Notifications.Email.Email
                {
                    From = "reserverations@marriott.com",
                    To = guestInformation.Email,
                    Subject = $"Reminder: Marriott Reservation # {reservation.ExternalId}",
                    Body = body.ToString()
                }
            });
        }
    }
}