using SomeStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeStore.Data.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllByCategory(Category category);
        IEnumerable<Product> GetAllFavourite();
        Product Get(int id);
        bool Delete(Product entity);
        bool Create(Product entity);
        bool Update(Product entity);
    }
}
