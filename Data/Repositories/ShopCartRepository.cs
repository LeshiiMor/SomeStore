using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Models;

namespace SomeStore.Data.Repositories
{
    public class ShopCartRepository : IShopCartRepository
    {
        private readonly ContextDB _context;
        public ShopCartRepository(ContextDB context)
        {
            _context = context;
        }
        public bool Create(ShopCart cart)
        {
            _context.ShopCarts.Add(cart);
            return (_context.SaveChanges()>0);
        }

        public ShopCart Get(int id)
        {
            return _context.ShopCarts.Include(p=>p.User).Include(p=>p.ShopCartItems).ThenInclude(p=>p.Product).FirstOrDefault(p=>p.Id == id);
        }

        public ShopCart Get(User user)
        {
            return _context.ShopCarts.Include(p => p.User).Include(p => p.ShopCartItems).ThenInclude(p=>p.Product).FirstOrDefault(p => p.UserId == user.Id);
        }

        public IEnumerable<ShopCart> GetAll()
        {
            return _context.ShopCarts.ToList();
        }

        public bool Remove(ShopCart cart)
        {
            _context.ShopCarts.Remove(cart);
            return (_context.SaveChanges() > 0);
        }

        public bool Update(ShopCart cart)
        {
            _context.ShopCarts.Update(cart);
            return (_context.SaveChanges() > 0);
        }
    }
}
