using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Marriott.ITOps.Invoicing.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.Invoicing.Send.Handlers
{
    public class GenerateInvoiceHandler : IHandleMessages<GenerateInvoice>
    {
        public async Task Handle(GenerateInvoice message, IMessageHandlerContext context)
        {
            var invoiceUrl = $"http://localhost:55215/Invoice/UrlAsPdf?url=http://localhost:55215/Invoice/Invoice?reservationId={message.ReservationId}";
            using (var httpClient = new HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(invoiceUrl);
                using (var output = new FileStream($@"C:\Users\mgmccarthy\Desktop\MarriottInvoices\Invoice_{message.ReservationId}.pdf", FileMode.Create))
                {
                    stream.CopyTo(output);
                }
            }
        }
    }
}