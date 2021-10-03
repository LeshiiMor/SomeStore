using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.Data.Interfaces
{
    public interface IOrderRepository
    {
        bool Create(Order order);
        bool Remove(Order order);
        bool Update(Order order);
        Order Get(int id);
        Order Get(Product product);
        Order Get(User us);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetAllByUser(User user);
        IEnumerable<Order> GetAllByProduct(Product product);
    }
}
