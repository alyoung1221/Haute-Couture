using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using Project1.Models;
using Project1.ViewModels;

namespace Project1.Controllers
{
    public class ProductsController : Controller
    {
        private Project1Context db = new Project1Context();

        // GET: Products
        public ActionResult Index(string category, string search, string order, int? size, int? page)
        {
            // instantiate a new view model
            ProductIndexViewModel viewModel = new ProductIndexViewModel();

            // select the products
            var products = db.Products.Include(p => p.Category);

            // perform the search and save the search string to the viewModel
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) ||
                p.Description.Contains(search) ||
                p.Category.CategoryName.Contains(search));
                viewModel.search = search;
            }

            // group search results into categories and count how many items in each category
            viewModel.CatsWithCount = from matchingProducts in products
                                      orderby matchingProducts.Category.CategoryName
                                      group matchingProducts by
                                               matchingProducts.Category.CategoryName into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Path == category);
                viewModel.category = category;
            }

            // sort the results            
            switch (order)
            {
                case "price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            int currentPage = (page ?? 1);
            viewModel.size = size;
            viewModel.Products = products.ToPagedList(currentPage, size ?? Constants.PageItems);
            viewModel.order = order;
            viewModel.filters = new Dictionary<string, string>
            {
                {"Low to high", "price" },
                {"High to low", "price-desc" }
            };
            return View(viewModel);
        }

        public ActionResult ProductsOnSale(string category, string search, string order, int? size, int? page)
        {
            // instantiate a new view model
            ProductIndexViewModel viewModel = new ProductIndexViewModel();

            // select the products
            var products = db.Products.Include(p => p.Category).Where(p => p.OnSale == true);

            // perform the search and save the search string to the viewModel
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.ProductName.Contains(search) ||
                p.Description.Contains(search) ||
                p.Category.CategoryName.Contains(search));
                viewModel.search = search;
            }

            // group search results into categories and count how many items in each category
            viewModel.CatsWithCount = from matchingProducts in products
                                      orderby matchingProducts.Category.CategoryName
                                      group matchingProducts by
                                               matchingProducts.Category.CategoryName into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Path == category);
                viewModel.category = category;
            }

            // sort the results            
            switch (order)
            {
                case "price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderByDescending(p => p.SaleEnd);
                    break;
            }

            int currentPage = (page ?? 1);
            viewModel.size = size;
            viewModel.Products = products.ToPagedList(currentPage, size ?? Constants.PageItems);
            viewModel.order = order;
            viewModel.filters = new Dictionary<string, string>
            {
                {"Low to high", "price" },
                {"High to low", "price-desc" }
            };
            return View(viewModel);
        }

        // GET: Products/Details/5
        [Route("shop/product/{id}")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Where(p => p.Path == id).FirstOrDefault();

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(c => c.CategoryName), "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https:// go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Description,Price,ReleaseDate,OnSale,Discount,SaleStart,SaleEnd,Path,CategoryID")] Product product, HttpPostedFileBase Image)
        {
            if (product.OnSale != false)
            {
                if (product.Discount == null)
                {
                    ModelState.AddModelError("Discount", "Please enter the discount amount.");
                }

                if (String.IsNullOrEmpty(Request["Sale"]) || Request["Sale"] == " to ")
                {
                    ModelState.AddModelError("Sale", "Please select the duration of the sale.");
                }
                else
                {
                    try
                    {
                        var date = Request["Sale"];
                        var saleStart = DateTime.Parse(date.Substring(0, date.IndexOf("to")));
                        var saleEnd = DateTime.Parse(date.Substring(date.IndexOf("to") + 3));
                        product.SaleStart = saleStart;
                        product.SaleEnd = saleEnd;
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("Sale", "Please select a range of dates.");
                    }
                }
            }

            //check the user has entered a file              
            if (Image != null)
            {
                if (ValidateExtension(Image))
                {
                    if (ValidateFile(Image))
                    {
                        try
                        {
                            SaveFileToDisk(Image);
                            product.Image = Image.FileName;
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError("Image", "An error occurred saving the image " + Image.FileName + " to disk. Please try again.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "The image " + Image.FileName + " must be smaller than 2MB.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Image", "The file must be .png, .jpg, or .jpeg.");
                }

            }
            else
            {
                // if the user has not entered a file           
                ModelState.AddModelError("Image", "Please select an image.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ReleaseDate = DateTime.Now;
                    product.Path = product.ProductName.ToString().ToLower().Replace(" ", "-");
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    SqlException innerException = ex.InnerException.InnerException as SqlException;

                    if (innerException != null && innerException.Number == 2627 || innerException.Number == 2601)
                    {
                        ModelState.AddModelError("ProductName", "The product " + product.ProductName + " already exists. Please enter a different name.");
                    }
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(c => c.CategoryName), "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Where(p => p.Path == id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(c => c.CategoryName), "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https:// go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase Image)
        {
            var currentProduct = db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault();

            if (product.OnSale != false)
            {
                if (product.Discount == null)
                {
                    ModelState.AddModelError("Discount", "Please enter the discount amount.");
                }
                else
                {
                    currentProduct.Discount = product.Discount;
                }

                if (String.IsNullOrEmpty(Request["Sale"]) || Request["Sale"] == " to ")
                {
                    ModelState.AddModelError("Sale", "Please select the duration of the sale.");
                }
                else
                {
                    try
                    {
                        var date = Request["Sale"];
                        var saleStart = DateTime.Parse(date.Substring(0, date.IndexOf("to")));
                        var saleEnd = DateTime.Parse(date.Substring(date.IndexOf("to") + 3));
                        product.SaleStart = saleStart;
                        product.SaleEnd = saleEnd;
                        currentProduct.SaleStart = product.SaleStart;
                        currentProduct.SaleEnd = product.SaleEnd;
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("Sale", "Please select a range of dates.");
                    }
                }
            }
            else
            {
                currentProduct.SaleStart = null;
                currentProduct.SaleEnd = null;
                currentProduct.Discount = null;
            }

            //check the user has entered a file              
            if (Image != null)
            {
                product.Image = Image.FileName;

                //check if the image is valid              
                if (ValidateExtension(Image))
                {
                    if (ValidateFile(Image))
                    {
                        try
                        {
                            SaveFileToDisk(Image);
                        }
                        catch (Exception)
                        {
                            ModelState.AddModelError("Image", "An error occurred saving the image " + Image.FileName + " to disk. Please try again.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "The image " + Image.FileName + " must be smaller than 2MB.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Image", "The file must be .jpg, .jpeg, or .png.");
                }
            }
            else
            {
                product.Image = currentProduct.Image;
                currentProduct.Image = currentProduct.Image;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null)
                    {
                        System.IO.File.Delete(Request.MapPath(Constants.ImagePath + currentProduct.Image));
                        currentProduct.Image = Image.FileName;
                    }

                    currentProduct.ProductName = product.ProductName;
                    currentProduct.Description = product.Description;
                    currentProduct.Price = product.Price;
                    currentProduct.OnSale = product.OnSale;
                    currentProduct.Path = product.ProductName.ToString().ToLower().Replace(" ", "-");
                    currentProduct.CategoryID = product.CategoryID;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    SqlException innerException = ex.InnerException.InnerException as SqlException;

                    if (innerException != null && innerException.Number == 2627 || innerException.Number == 2601)
                    {
                        ModelState.AddModelError("ProductName", "The product " + product.ProductName + " already exists. Please enter a different name.");
                    }
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(c => c.CategoryName), "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Where(p => p.Path == id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Where(p => p.Path == id).FirstOrDefault();
            System.IO.File.Delete(Request.MapPath(Constants.ImagePath + product.Image));
            db.Products.Remove(product);
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

        private bool ValidateExtension(HttpPostedFileBase Image)
        {
            string fileExtension = System.IO.Path.GetExtension(Image.FileName).ToLower();
            string[] allowedFileTypes = { ".png", ".jpeg", ".jpg" };

            if (allowedFileTypes.Contains(fileExtension))
            {
                return true;
            }
            return false;
        }

        private bool ValidateFile(HttpPostedFileBase Image)
        {
            if (Image.ContentLength > 0 && Image.ContentLength < 2097152)
            {
                return true;
            }
            return false;
        }

        private bool CheckOrientation(WebImage img)
        {
            if (img.Width < img.Height)
            {
                return true;
            }
            return false;
        }

        private bool CheckDimensions(WebImage img)
        {
            if (img.Width >= 600 && img.Height >= 900)
            {
                return true;
            }
            return false;
        }

        private void SaveFileToDisk(HttpPostedFileBase Image)
        {
            WebImage img = new WebImage(Image.InputStream);

            Product DuplicateImage = db.Products.Where(cm => string.Compare(cm.Image, Image.FileName, true) == 0).FirstOrDefault();

            if (DuplicateImage != null)
            {
                ModelState.AddModelError("Image", "The image " + Image.FileName + " already exists. Please select a different image.");
            }
            else
            {
                if (CheckOrientation(img))
                {
                    if (!CheckDimensions(img))
                    {
                        ModelState.AddModelError("Image", "The image must be at least 600px x 900px.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Image", "The image orientation must be portrait.");
                }
            }

            if (ModelState.IsValid)
            {
                if (img.Width > 600)
                {
                    img.Resize(600, 900, false).Crop(1, 1);
                }
                img.Save(Constants.ImagePath + Image.FileName);
            }
        }
    }
}