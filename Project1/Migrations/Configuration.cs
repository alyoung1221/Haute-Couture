namespace Project1.Migrations
{
    using Project1.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project1.Models.Project1Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Project1.Models.Project1Context";
        }

        protected override void Seed(Project1.Models.Project1Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var categories = new List<Category>();

            if (!context.Categories.Any())
            {
                categories.Add(new Category() { CategoryName = "Accessories", Path = "Accessories" });
                categories.Add(new Category() { CategoryName = "Shoes", Path = "shoes" });
                categories.Add(new Category() { CategoryName = "Teens", Path = "teens" });
                categories.Add(new Category() { CategoryName = "Women", Path = "women" });

                categories.ForEach(c => context.Categories.AddOrUpdate(i => i.CategoryName, c));
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product()
                    {
                        ProductName = "Green Apple Purse",
                        Description = "A vibrant green apple purse that brightens any outfit",
                        Price = 15.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Wednesday),
                        Image = "green-apple-purse.png",
                        Path = "green-apple-purse",
                        CategoryID = categories.Single(c => c.CategoryName == "Accessories").CategoryID
                    },
                    new Product() {
                        ProductName = "Nude Hair Bow",
                        Description = "Classy nude hair bow",
                        Price = 15.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek),
                        Image = "nude-hair-bow.png",
                        Path = "nude-hair-bow",
                        CategoryID = categories.Single(c => c.CategoryName == "Accessories").CategoryID
                    },
                    new Product() {
                        ProductName = "Ruby Tassel Earrings",
                        Description = "Elegant ruby tassel earrings accented with small gold hoops",
                        Price = 20.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek),
                        OnSale = true,
                        SaleStart = DateTime.Now,
                        SaleEnd = DateTime.Now.AddDays(7),
                        Discount = 25,
                        Image = "ruby-tassel-earrings.png",
                        Path = "ruby-tassel-earrings",
                        CategoryID = categories.Single(c => c.CategoryName == "Accessories").CategoryID
                    },
                    new Product() {
                        ProductName = "Strawberry Pink Backpack",
                        Description = "Vintage pink strawberry backpack with tassels",
                        Price = 21.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek),
                        Image = "strawberry-pink-backpack.png",
                        Path = "strawberry-pink-backpack",
                        CategoryID = categories.Single(c => c.CategoryName == "Accessories").CategoryID
                    },
                    new Product() {
                        ProductName = "Navy Ribbon Hat",
                        Description = "Beachy visor hat with navy ribbons",
                        Price = 18.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek),
                        Image = "navy-ribbon-hat.png",
                        Path = "navy-ribbon-hat",
                        CategoryID = categories.Single(c => c.CategoryName == "Accessories").CategoryID
                    },
                    new Product() {
                        ProductName = "Black Knee-High Boots",
                        Description = "Chic knee-high heeled black boots",
                        Price = 25.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek),
                        Image = "black-knee-high-boots.png",
                        Path = "black-knee-high-boots",
                        CategoryID = categories.Single(c => c.CategoryName == "Shoes").CategoryID
                    },
                    new Product() {
                        ProductName = "Nude Gladiator Heels",
                        Description = "Classy and modern nude gladiator heels",
                        Price = 27.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday),
                        OnSale = true,
                        SaleStart = DateTime.Now,
                        SaleEnd = DateTime.Now.AddDays(7),
                        Discount = 25,
                        Image = "nude-gladiator-heels.png",
                        Path = "nude-gladiator-heels",
                        CategoryID = categories.Single(c => c.CategoryName == "Shoes").CategoryID
                    },
                    new Product() {
                        ProductName = "Black Bomb Jacket",
                        Description = "Hoodie black bomb jacket with decorative metal buttons",
                        Price = 19.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek),
                        Image = "black-bomb-jacket.png",
                        Path = "black-bomb-jacket",
                        CategoryID = categories.Single(c => c.CategoryName == "Teens").CategoryID
                    },
                    new Product() {
                        ProductName = "Autumn Chevron Sweater",
                        Description = "An orange and white chevron sweater that adds a pop of colour to any outfit",
                        Price = 25.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Tuesday),
                        OnSale = true,
                        SaleStart = DateTime.Now,
                        SaleEnd = DateTime.Now.AddDays(7),
                        Discount = 25,
                        Image = "autumn-chevron-sweater.png",
                        Path = "autumn-chevron-sweater",
                        CategoryID = categories.Single(c => c.CategoryName == "Teens").CategoryID
                    },
                    new Product() {
                        ProductName = "Cherry Print Skirt",
                        Description = "A dark navy skirt featuring cherry patterns",
                        Price = 20.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday),
                        OnSale = true,
                        SaleStart = DateTime.Now,
                        SaleEnd = DateTime.Now.AddDays(7),
                        Discount = 25,
                        Image = "cherry-print-skirt.png",
                        Path = "cherry-print-skirt",
                        CategoryID = categories.Single(c => c.CategoryName == "Teens").CategoryID
                    },
                    new Product() {
                        ProductName = "Black Striped Sweater",
                        Description = "A black and white striped comfortable fitted sweater",
                        Price = 19.00M,
                        ReleaseDate = DateTime.Now.AddDays(-(int)DateTime.Today.DayOfWeek),
                        Image = "black-striped-sweater.png",
                        Path = "black-striped-sweater",
                        CategoryID = categories.Single(c => c.CategoryName == "Teens").CategoryID
                    },
                    new Product() {
                        ProductName = "Lacey White Dress",
                        Description = "An elegant, flowy white lacey dress",
                        Price = 22.00M,
                        ReleaseDate = DateTime.Now,
                        Image = "lacey-white-dress.png",
                        Path = "lacey-white-dress",
                        CategoryID = categories.Single(c => c.CategoryName == "Women").CategoryID
                    },
                    new Product() {
                        ProductName = "Navy Sleeveless Blouse",
                        Description = "Loosely fitted summer dark navy blouse",
                        Price = 17.00M,
                        ReleaseDate = DateTime.Now,
                        Image = "navy-sleeveless-blouse.png",
                        Path = "navy-sleeveless-blouse",
                        CategoryID = categories.Single(c => c.CategoryName == "Women").CategoryID
                    },
                    new Product() {
                        ProductName = "Sheer Flowy Blouse",
                        Description = "Light summery sheer blouse",
                        Price = 17.00M,
                        ReleaseDate = DateTime.Now,
                        Image = "sheer-flowy-blouse.png",
                        Path = "sheer-flowy-blouse",
                        CategoryID = categories.Single(c => c.CategoryName == "Women").CategoryID
                    },
                    new Product() {
                        ProductName = "Cherry Maxi",
                        Description = "Statement chery red maxi",
                        Price = 22.00M,
                        ReleaseDate = DateTime.Now,
                        Image = "cherry-maxi.jpg",
                        Path = "cherry-maxi",
                        CategoryID = categories.Single(c => c.CategoryName == "Women").CategoryID
                    }
                };

                products.ForEach(c => context.Products.AddOrUpdate(p => p.ProductName, c));
                context.SaveChanges();
            }
        }
    }
}
