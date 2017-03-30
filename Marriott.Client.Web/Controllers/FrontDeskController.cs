using System.Web.Mvc;
using Marriott.Client.Web.Extensions;

namespace Marriott.Client.Web.Controllers
{
    public class FrontDeskController : Controller
    {
        public FrontDeskController()
        {
            ViewBag.ActiveNavBarItem = NavigationBarItem.FrontDesk;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}