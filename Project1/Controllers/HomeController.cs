using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project1.Models;
using Project1.ViewModels;

namespace Project1.Controllers
{
    public class HomeController : Controller
    {
        private Project1Context db = new Project1Context();

        public ActionResult Index()
        {
            // select the products
            return View(db.Products.OrderByDescending(p => p.ReleaseDate).Take(6).ToList());
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            return View();
        }
    }
}