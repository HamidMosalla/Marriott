using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Marriott.Client.Web.Composers.CheckOut;
using Rotativa;

namespace Marriott.Client.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly CheckOutComposer composer;

        public InvoiceController(CheckOutComposer  composer)
        {
            this.composer = composer;
        }

        [HttpGet]
        public async Task<ActionResult> Invoice(Guid reservationId)
        {
            var model = await composer.ComposeVerifyInvoice(reservationId);
            return View(model);
        }

        //https://www.nuget.org/packages/Rotativa/
        //https://github.com/webgio/Rotativa
        //https://rotativahq.com/
        //https://rotativahq.com/dashboard/home

        //http://localhost:55215/Invoice/Invoice?reservationId=221ee259-8de4-462b-b9e3-6a3fbc20577b
        //http://localhost:55215/Invoice/UrlAsPdf?url=http://localhost:55215/Invoice/Invoice?reservationId=221ee259-8de4-462b-b9e3-6a3fbc20577b
        //http://localhost:55215/Invoice/SaveInvoiceAsPdfUsingHttpClient
        public ActionResult UrlAsPdf(string url)
        {
            return new UrlAsPdf(url);
        }

        //public async Task<HttpStatusCode> SaveInvoiceAsPdfUsingHttpClient()
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var stream = await httpClient.GetStreamAsync("http://localhost:55215/Invoice/UrlAsPdf?url=http://localhost:55215/Invoice/Invoice?reservationId=221ee259-8de4-462b-b9e3-6a3fbc20577b");
        //        using (var output = new FileStream(@"D:\Projects\Marriott\Marriott.Client.Web\Controllers\Invoice_221ee259-8de4-462b-b9e3-6a3fbc20577b.pdf", FileMode.Create))
        //        {
        //            stream.CopyTo(output);
        //        }
        //    }
            
        //    //202 b/c evntually, this will all be async
        //    return HttpStatusCode.Accepted;
        //}
    }
}