using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Models;

namespace SomeStore.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ContextDB _context;
        public CategoryRepository(ContextDB context)
        {
            _context = context;
        }
        public bool Create(Category category)
        {
            _context.Categories.Add(category);
            return (_context.SaveChanges()>0);
        }

        public bool Delete(Category category)
        {
            _context.Categories.Remove(category);
            return (_context.SaveChanges() > 0);
        }

        public Category Get(int id)
        {
            return _context.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }
    }
}
