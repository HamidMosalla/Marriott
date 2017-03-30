using System;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Commands;
using Marriott.Business.Reservation.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    //Reservation Pattern (http://arnon.me/soa-patterns/reservation/)
    public class ReservationSaga : Saga<ReservationSaga.SagaData>,
        IAmStartedByMessages<PendingReservationCreated>,
        IHandleMessages<ConfirmPendingReservation>,
        IHandleMessages<CancelPendingReservation>,
        IHandleTimeouts<ReservationSaga.TimeoutState>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<PendingReservationCreated>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
            mapper.ConfigureMapping<ConfirmPendingReservation>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
            mapper.ConfigureMapping<CancelPendingReservation>(msg => msg.ReservationId).ToSaga(data => data.ReservationId);
        }

        public async Task Handle(PendingReservationCreated message, IMessageHandlerContext context)
        {
            await RequestTimeout<TimeoutState>(context, DateTime.Now.AddMinutes(20));
        }

        public async Task Handle(ConfirmPendingReservation message, IMessageHandlerContext context)
        {
            await context.Send(new ConfirmReservation { ReservationId = Data.ReservationId });
            MarkAsComplete();
        }

        public async Task Handle(CancelPendingReservation message, IMessageHandlerContext context)
        {
            await PublishPendingReservationExpiredAndMarkComplete(context);
        }

        public async Task Timeout(TimeoutState state, IMessageHandlerContext context)
        {
            await PublishPendingReservationExpiredAndMarkComplete(context);
        }

        private async Task PublishPendingReservationExpiredAndMarkComplete(IMessageHandlerContext context)
        {
            await context.Publish(new PendingReservationExpired { ReservationId = Data.ReservationId});
            MarkAsComplete();
        }

        public class SagaData : ContainSagaData
        {
            public Guid ReservationId { get; set; }
        }

        public class TimeoutState { }
    }
}