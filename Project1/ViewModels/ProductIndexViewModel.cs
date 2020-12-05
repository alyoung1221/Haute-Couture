using PagedList;
using Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project1.ViewModels
{
    public class ProductIndexViewModel
    {
        public IPagedList<Product> Products { get; set; }
        public int? size { get; set; }
        public string search { get; set; }
        public IEnumerable<CategoryWithCount> CatsWithCount { get; set; }
        public string category { get; set; }
        public string order { get; set; }
        public Dictionary<string, string> filters { get; set; }

        public IEnumerable<SelectListItem> categories
        {
            get
            {
                var categories = new List<SelectListItem>();

                categories.Add(new SelectListItem { Text = "- Select one -", Value = "", Disabled = true, Selected = true });

                foreach (var category in CatsWithCount)
                {
                    categories.Add(new SelectListItem
                    {
                        Value = category.CategoryName,
                        Text = category.CatNameWithCount
                    });
                }

                return categories;
            }
        }
    }

    public class CategoryWithCount
    {
        public int ProductCount { get; set; }
        public string CategoryName { get; set; }
        public string CatNameWithCount
        {
            get
            {
                return CategoryName + " (" + ProductCount.ToString() + ")";
            }
        }
    }
}