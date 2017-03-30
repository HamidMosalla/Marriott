using System;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Events;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    //http://elliott.org/the-troubleshooter/missed-my-hotel-reservation-by-a-month-do-i-still-have-to-pay/
    //the fee should be the cost of the room for one night's stay
    public class ReservationNoShowPolicy : Saga<ReservationNoShowPolicy.SagaData>,
        IAmStartedByMessages<ReservationConfirmed>,
        IHandleMessages<GuestCheckedIn>,
        IHandleMessages<ReservationCancelled>,
        IHandleMessages<RoomAllocationFailed>,
        IHandleTimeouts<ReservationNoShowPolicy.TimeoutState>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<ReservationConfirmed>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
            mapper.ConfigureMapping<GuestCheckedIn>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
            mapper.ConfigureMapping<ReservationCancelled>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
            mapper.ConfigureMapping<RoomAllocationFailed>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
        }

        public async Task Handle(ReservationConfirmed message, IMessageHandlerContext context)
        {
            await RequestTimeout<TimeoutState>(context, ThreePmTheDayAfter(message.CheckIn));
        }

        public Task Handle(GuestCheckedIn message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        public Task Handle(ReservationCancelled message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        public Task Handle(RoomAllocationFailed message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        public async Task Timeout(TimeoutState state, IMessageHandlerContext context)
        {
            await context.Publish(new ReservationNoShowPolicyViolated { ReservationId = Data.ReservationId });
            MarkAsComplete();
        }

        private DateTime ThreePmTheDayAfter(DateTime checkIn)
        {
            return DateTime.SpecifyKind(checkIn.Date.AddDays(1).AddHours(15), DateTimeKind.Local);
        }

        public class SagaData : ContainSagaData
        {
            public Guid ReservationId { get; set; }
        }

        public class TimeoutState { }
    }
}