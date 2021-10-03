using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SomeStore.ViewModels
{
    public class IndexProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public SelectList Categories { get; set; }
    }
}
