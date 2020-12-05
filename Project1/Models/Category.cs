using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project1.Models
{
    public class Category
    {
        [Display(Name = "Category ID")]
        public int CategoryID { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please enter the category name.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "The category name must be between 5 and 50 characters.")]
        [Index(IsUnique = true)]
        public string CategoryName { get; set; }
        public string Path { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}