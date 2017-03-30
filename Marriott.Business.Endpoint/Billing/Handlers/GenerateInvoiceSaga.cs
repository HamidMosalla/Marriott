using System;
using System.Threading.Tasks;
using Marriott.External.Events;
using Marriott.ITOps.Invoicing.Commands;
using NServiceBus;

namespace Marriott.Business.Endpoint.Billing.Handlers
{
    public class GenerateInvoiceSaga : Saga<GenerateInvoiceSaga.SagaData>,
        IAmStartedByMessages<GuestCheckedIn>,
        IHandleMessages<GuestCheckedOut>,
        IHandleMessages<GuestStayExtended>,
        IHandleTimeouts<GenerateInvoiceSaga.TimeoutState>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<GuestCheckedIn>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
            mapper.ConfigureMapping<GuestCheckedOut>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
            mapper.ConfigureMapping<GuestStayExtended>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
        }

        public async Task Handle(GuestCheckedIn message, IMessageHandlerContext context)
        {
            Data.CheckOutDate = message.CheckOutDate;
            await RequestTimeout<TimeoutState>(context, ElevenPmTheNightBefore(message.CheckOutDate));
        }

        public async Task Handle(GuestCheckedOut message, IMessageHandlerContext context)
        {
            //if GuestCheckedOut arrives before the timeout, then immediately send the GenerateInvoice command and MarkAsComplete()
            await SendGenerateInvoiceAndMarkAsComplete(context);
        }

        public Task Handle(GuestStayExtended message, IMessageHandlerContext context)
        {
            //if GuestStayExtended arrives before the timeout fires, populate Data.NewCheckOutDate
            Data.NewCheckOutDate = message.NewCheckOutDate;
            return Task.CompletedTask;
        }

        public async Task Timeout(TimeoutState state, IMessageHandlerContext context)
        {
            //check to see if NewCheckOutDate has been set (it's not DateTime.MinValue). NewCheckOutDate is set by GuestStayExtended
            //if NewCheckOutDate has been set, read that value into a local variable, then
            //set the CheckOutDate to the NewCheckOutDate, and reset the NewCheckOutDate back to DateTime.MinValue
            //essentially, we're resetting the Saga timeout/data for the next time the Timeout method is called.
            //this will allow us to continually process any number of extend stay requests by the guest for a given ReservationId
            if (Data.NewCheckOutDate != DateTime.MinValue)
            {
                var newCheckOutDate = Data.NewCheckOutDate;

                Data.CheckOutDate = Data.NewCheckOutDate;
                Data.NewCheckOutDate = DateTime.MinValue;

                await RequestTimeout<TimeoutState>(context, ElevenPmTheNightBefore(newCheckOutDate));
            }
            else
            {
                await SendGenerateInvoiceAndMarkAsComplete(context);
            }
        }

        private DateTime ElevenPmTheNightBefore(DateTime checkOut)
        {
            return DateTime.SpecifyKind(checkOut.Date.AddDays(-1).AddHours(23), DateTimeKind.Local);
        }

        private async Task SendGenerateInvoiceAndMarkAsComplete(IMessageHandlerContext context)
        {
            await context.Send(new GenerateInvoice { ReservationId = Data.ReservationId });
            MarkAsComplete();
        }

        public class SagaData : ContainSagaData
        {
            public Guid ReservationId { get; set; }
            public DateTime CheckOutDate { get; set; }
            public DateTime NewCheckOutDate { get; set; }
        }

        public class TimeoutState { }
    }
}
