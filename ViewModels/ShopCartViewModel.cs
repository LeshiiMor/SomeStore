using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.ViewModels
{
    public class ShopCartViewModel
    {
        public ShopCart ShopCart { get; set; }
        public double FinalPrice { get; set; } = 0;
        public ShopCartViewModel()
        {
            ShopCart = new ShopCart();
        }
    }
}
