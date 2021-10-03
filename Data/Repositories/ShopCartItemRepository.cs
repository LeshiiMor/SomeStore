using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;
using SomeStore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SomeStore.Data.Repositories
{
    public class ShopCartItemRepository : IShopCartItemRepository
    {
        private readonly ContextDB _context;
        public ShopCartItemRepository(ContextDB context)
        {
            _context = context;
        }
        public bool Create(ShopCartItem item)
        {
            _context.ShopCartItems.Add(item);
            return (_context.SaveChanges()>0);
        }

        public ShopCartItem Get(int id)
        {
            return _context.ShopCartItems.Include(p => p.Product).FirstOrDefault(p=>p.Id == id);
        }

        public ShopCartItem Get(Product product)
        {
            return _context.ShopCartItems.Include(p => p.Product).FirstOrDefault(p => p.ProductId == product.Id);
        }

        public IEnumerable<ShopCartItem> GetAll()
        {
            return _context.ShopCartItems.Include(p => p.Product);
        }

        public IEnumerable<ShopCartItem> GetAllByShopCart(ShopCart shopCart)
        {
            return _context.ShopCartItems.Include(p => p.Product).Where(p=>p.ShopCartId ==shopCart.Id);
        }

        public bool Remove(ShopCartItem item)
        {
            _context.ShopCartItems.Remove(item);
            return (_context.SaveChanges() > 0);
        }

        public bool Update(ShopCartItem item)
        {
            _context.ShopCartItems.Update(item);
            return (_context.SaveChanges() > 0);
        }
    }
}
