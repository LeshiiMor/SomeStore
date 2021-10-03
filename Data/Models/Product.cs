using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeStore.Data.Models;

namespace SomeStore.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Укажите название продукта")]
        public string Name { get; set; }
        
        [Required(ErrorMessage ="Укажите описание продукта")]
        [Column(TypeName ="varchar(200)")]
        public string Description { get; set; }

        public string ImageURL { get; set; } = "/Content/product-icon.png";
        public bool IsFavourite { get; set; } = false;

        [Required]
        public double Price { get; set; } = 0;
        
        [Required]
        public  int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
