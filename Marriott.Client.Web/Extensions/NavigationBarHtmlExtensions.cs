using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Marriott.Client.Web.Extensions
{
    public class NavigationBarItem
    {
        public static string Reservations { get; } = "Reservations";
        public static string FrontDesk { get; } = "Front Desk";
        public static string GuestServices { get; } = "Guest Services";
        public static string Housekeeping { get; } = "Housekeeping";
        public static string Marketing { get; } = "Marketing";

        public string LinkText { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public bool Active { get; set; }
    }

    public static class NavigationBarHtmlExtensions
    {
        public static List<NavigationBarItem> NavigationBarItems()
        {
            return new List<NavigationBarItem>
            {
                new NavigationBarItem { LinkText = NavigationBarItem.Reservations, ActionName = "Search", ControllerName = "Reservation" },
                new NavigationBarItem { LinkText = NavigationBarItem.FrontDesk, ActionName = "Index", ControllerName = "FrontDesk" },
                new NavigationBarItem { LinkText = NavigationBarItem.GuestServices, ActionName = "Index", ControllerName = "GuestServices" },
                new NavigationBarItem { LinkText = NavigationBarItem.Housekeeping, ActionName = "Index", ControllerName = "Housekeeping" },
                new NavigationBarItem { LinkText = NavigationBarItem.Marketing, ActionName = "Index", ControllerName = "Marketing" }
            };
        }

        public static MvcHtmlString RenderNavigationBar(this HtmlHelper htmlHelper)
        {
            var activeNavBarItem = htmlHelper.ViewBag.ActiveNavBarItem;
            var navSb = new StringBuilder();

            var navBar = new TagBuilder("ul");
            navBar.AddCssClass("nav navbar-nav");
            foreach (var item in NavigationBarItems())
            {
                var navBarItem = new TagBuilder("li");
                if (activeNavBarItem != null)
                {
                    if (activeNavBarItem.ToString() == item.LinkText)
                    {
                        navBarItem.AddCssClass("active");
                    }
                }
                navBarItem.InnerHtml = $"<a href=\"/{item.ControllerName}/{item.ActionName}\">{item.LinkText}</a>";
                navBar.InnerHtml += navBarItem.ToString();
            }
            navSb.Append(navBar);
            return MvcHtmlString.Create(navSb.ToString());
        }
    }
}