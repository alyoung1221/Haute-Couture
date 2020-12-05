using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project1.Models;

namespace Project1.Controllers
{
    public class CategoriesController : Controller
    {
        private Project1Context db = new Project1Context();

        // GET: Categories
        [Route("shop/collections")]
        public ActionResult Index()
        {
            return View(db.Categories.OrderBy(c => c.CategoryName).ToList());
        }

        // GET: Categories/Details/5
        [Route("categories/{id}")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Where(c => c.Path == id).FirstOrDefault();
            int catID = category.CategoryID;

            ViewBag.numProducts = db.Products.Where(p => p.CategoryID == catID).ToList().Count();
            ViewBag.products = db.Products.Where(p => p.CategoryID == catID).OrderByDescending(p => p.ReleaseDate).Take(4).ToList();

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,Path")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    category.Path = category.CategoryName.ToString().ToLower().Replace(" ", "-");
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    SqlException innerException = ex.InnerException.InnerException as SqlException;

                    if (innerException != null && innerException.Number == 2627 || innerException.Number == 2601)
                    {
                        ModelState.AddModelError("CategoryName", "The category " + category.CategoryName + " already exists.");
                    }
                }
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Where(c => c.Path == id).FirstOrDefault();
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Path")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    category.Path = category.CategoryName.ToString().ToLower().Replace(" ", "-");
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    SqlException innerException = ex.InnerException.InnerException as SqlException;

                    if (innerException != null && innerException.Number == 2627 || innerException.Number == 2601)
                    {
                        ModelState.AddModelError("CategoryName", "The category " + category.CategoryName + " already exists.");
                    }
                }
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Where(c => c.Path == id).FirstOrDefault();
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Category category = db.Categories.Where(c => c.Path == id).FirstOrDefault();
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}