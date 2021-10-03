using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SomeStore.Data.Models
{
    public class ShopCart
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; }
        public ShopCart()
        {
            ShopCartItems = new List<ShopCartItem>();
        }
    }
}
