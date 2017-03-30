using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Marriott.Client.Web.Models
{
    public static class CreditCardSelectListService
    {
        public static List<SelectListItem> GetMonthList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "01" },
                new SelectListItem { Value = "2", Text = "02" },
                new SelectListItem { Value = "3", Text = "03" },
                new SelectListItem { Value = "4", Text = "04" },
                new SelectListItem { Value = "5", Text = "05" },
                new SelectListItem { Value = "6", Text = "06" },
                new SelectListItem { Value = "7", Text = "07" },
                new SelectListItem { Value = "8", Text = "08" },
                new SelectListItem { Value = "9", Text = "09" },
                new SelectListItem { Value = "10", Text = "10" },
                new SelectListItem { Value = "11", Text = "11" },
                new SelectListItem { Value = "12", Text = "12" }
            };
        }

        public static List<SelectListItem> GetYearList()
        {
            var selectListItems = new List<SelectListItem>();
            var year = DateTime.Now.Year;
            for (var i = 0; i <= 12; i++)
            {
                selectListItems.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
                year++;
            }
            return selectListItems;
        }
    }
}