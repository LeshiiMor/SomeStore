using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Models;

namespace SomeStore.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ContextDB _context;
        public ProductRepository(ContextDB context)
        {
            _context = context;
        }
        public bool Create(Product entity)
        {
            _context.Products.Add(entity);
            return (_context.SaveChanges() > 0);
        }

        public bool Delete(Product entity)
        {
            _context.Products.Remove(entity);
            return (_context.SaveChanges() > 0);
        }

        public Product Get(int id)
        {
            return _context.Products.Include(p=>p.Category).FirstOrDefault(p=>p.Id==id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        public IEnumerable<Product> GetAllByCategory(Category category)
        {
            return _context.Products.Include(p => p.Category).Where(p => p.CategoryId == category.Id).ToList();
        }

        public IEnumerable<Product> GetAllFavourite()
        {
            return _context.Products.Include(p => p.Category).Where(p => p.IsFavourite).ToList();
        }

        public bool Update(Product entity)
        {
            _context.Products.Update(entity);
            return (_context.SaveChanges()>0);
        }
    }
}
