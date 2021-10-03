using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.Data.Interfaces
{
    public interface IShopCartItemRepository
    {
        bool Create(ShopCartItem item);
        bool Remove(ShopCartItem item);
        bool Update(ShopCartItem item);
        ShopCartItem Get(int id);
        ShopCartItem Get(Product product);
        IEnumerable<ShopCartItem> GetAllByShopCart(ShopCart shopCart);
        IEnumerable<ShopCartItem> GetAll();
    }
}
