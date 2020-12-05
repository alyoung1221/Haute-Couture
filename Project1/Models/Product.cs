using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project1.Models
{
    public class Product
    {
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }
        [Display(Name = "Product")]
        [Required(ErrorMessage = "Please enter the product name.")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "The product name must be between 10 and 50 characters.")]
        [Index(IsUnique = true)]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Please enter the product description.")]
        [StringLength(100, MinimumLength = 15, ErrorMessage = "The description must be between 15 and 100 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter the price.")]
        [Range(10, 500, ErrorMessage = "The price must be between $10.00 and $500.00.")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "On Sale")]
        public bool OnSale { get; set; }
        [Display(Name = "Sale Start")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public DateTime? SaleStart { get; set; }
        [Display(Name = "Sale End")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public DateTime? SaleEnd { get; set; }
        [DisplayFormat(DataFormatString = "{0}%")]
        [Range(15, 70, ErrorMessage = "The discount must be between 15% and 70%.")]
        public int? Discount { get; set; }
        public string Image { get; set; }
        public string Path { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}