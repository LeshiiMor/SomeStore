using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.ViewModels
{
    public class CartViewModel
    {
        public double FinalPrice { get; set; }
        public ShopCart UserCart { get; set; }
    }
}
