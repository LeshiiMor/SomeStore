using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.Data.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        IEnumerable<User> GetAllByRole(Role role);
        User Get(int id);
        User Get(string username);
        bool Create(User user);
        bool Delete(User user);
        bool Update(User user);
    }
}
