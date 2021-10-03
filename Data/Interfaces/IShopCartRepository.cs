using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.Data.Interfaces
{
    public interface IShopCartRepository
    {
        bool Create(ShopCart cart);
        bool Remove(ShopCart cart);
        bool Update(ShopCart cart);
        IEnumerable<ShopCart> GetAll();
        ShopCart Get(int id);
        ShopCart Get(User user);
    }
}
