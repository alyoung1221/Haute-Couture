using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project1.Models
{
    public class Project1Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Project1Context() : base("name=Project1Context")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Project1Context, Migrations.Configuration>());
        }

        public System.Data.Entity.DbSet<Project1.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Project1.Models.Category> Categories { get; set; }
    }
}
