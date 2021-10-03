using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.Data.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll();
        Role Get(int id);
        bool Create(Role role);
        bool Delete(Role role);
        bool Update(Role role);
    }
}
