using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Models;

namespace SomeStore.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ContextDB _context;
        public OrderRepository(ContextDB context)
        {
            _context = context;
        }
        public bool Create(Order order)
        {
            _context.Orders.Add(order);
            return (_context.SaveChanges()>0);
        }

        public Order Get(int id)
        {
            return _context.Orders.Include(p => p.User).Include(p => p.Product).ThenInclude(c=>c.Category).FirstOrDefault(p=>p.Id == id);
        }

        public Order Get(Product product)
        {
            return _context.Orders.Include(p => p.User).Include(p => p.Product).ThenInclude(c => c.Category).FirstOrDefault(p => p.ProductId == product.Id);
        }

        public Order Get(User us)
        {
            return _context.Orders.Include(p => p.User).Include(p => p.Product).ThenInclude(c => c.Category).FirstOrDefault(p => p.UserId == us.Id); ;
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.Include(p=>p.User).Include(p=>p.Product).ThenInclude(c => c.Category);
        }

        public IEnumerable<Order> GetAllByProduct(Product product)
        {
            return _context.Orders.Include(p => p.User).Include(p => p.Product).ThenInclude(c => c.Category).Where(p=>p.ProductId == product.Id);
        }

        public IEnumerable<Order> GetAllByUser(User user)
        {
            return _context.Orders.Include(p => p.User).Include(p => p.Product).ThenInclude(c => c.Category).Where(p => p.UserId == user.Id);
        }

        public bool Remove(Order order)
        {
            _context.Orders.Remove(order);
            return (_context.SaveChanges() > 0);
        }

        public bool Update(Order order)
        {
            _context.Orders.Update(order);
            return (_context.SaveChanges() > 0);
        }
    }
}
