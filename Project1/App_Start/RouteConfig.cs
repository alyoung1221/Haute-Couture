using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
              name: "ProductsCreate",
              url: "shop/new-product",
              defaults: new { controller = "Products", action = "Create" }
            );

            routes.MapRoute(
              name: "CategoriesCreate",
              url: "shop/new-category",
              defaults: new { controller = "Categories", action = "Create" }
            );

            routes.MapRoute(
              name: "ProductsEdit",
              url: "shop/{id}/edit",
              defaults: new { controller = "Products", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "ProductsDelete",
              url: "shop/{id}/delete",
              defaults: new { controller = "Products", action = "delete" }
            );

            routes.MapRoute(
              name: "ProductsOnSaleByCategoryByPage",
              url: "shop/sales/collection/{category}/{page}",
              defaults: new { controller = "Products", action = "ProductsOnSale" }
            );

            routes.MapRoute(
              name: "ProductsOnSaleByCategory",
              url: "shop/sales/collection/{category}",
              defaults: new { controller = "Products", action = "ProductsOnSale" }
            );

            routes.MapRoute(
              name: "ProductsOnSaleByPage",
              url: "shop/sales/{page}",
              defaults: new { controller = "Products", action = "ProductsOnSale" }
            );

            routes.MapRoute(
              name: "ProductsOnSale",
              url: "shop/sales",
              defaults: new { controller = "Products", action = "ProductsOnSale" }
            );

            routes.MapRoute(
              name: "ProductsbyCategorybyPage",
              url: "shop/collection/{category}/{page}",
              defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
              name: "ProductsbyPage",
              url: "shop/{page}",
              defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
              name: "ProductsbyCategory",
              url: "shop/collection/{category}",
              defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
              name: "ProductsIndex",
              url: "shop",
              defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}