using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SomeStore.ViewModels
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage ="Укажите название продукта")]
        public string NameProduct { get; set; }

        [Required(ErrorMessage ="Укажите описание продукта")]
        public string Description { get; set; }

        public string ImageURL { get; set; } = "/Content/product-icon.png";
        public bool IsFavourite { get; set; } = false;
        public IFormFile ProductPhoto { get; set; }

        [Required(ErrorMessage = "Укажите цену продукта")]
        public double Price { get; set; } = 0;
        public int CategoryId { get; set; }
        public SelectList Categories { get; set; }
    }
}
