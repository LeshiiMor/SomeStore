using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeStore.Data.Models
{
    public class ShopCartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int ShopCartId { get; set; }
    }
}
