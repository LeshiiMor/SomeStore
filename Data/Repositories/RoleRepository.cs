using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data;
using SomeStore.Data.Models;
using SomeStore.Data.Interfaces;

namespace SomeStore.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public readonly ContextDB _context;
        public RoleRepository(ContextDB context)
        {
            _context = context;
        }
        public bool Create(Role role)
        {
            _context.Roles.Add(role);
            return (_context.SaveChanges() > 0);
        }

        public bool Delete(Role role)
        {
            _context.Roles.Remove(role);
            return (_context.SaveChanges()>0);
        }

        public bool Update(Role role)
        {
            _context.Roles.Update(role);
            return (_context.SaveChanges() > 0);
        }

        public Role Get(int id)
        {
            return _context.Roles.Find(id);
        }

        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.ToList();
        }
    }
}
