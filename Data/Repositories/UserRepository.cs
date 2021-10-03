using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SomeStore.Data;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Models;

namespace SomeStore.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextDB _context;
        public UserRepository(ContextDB context)
        {
            _context = context;
        }
        public bool Create(User user)
        {
            _context.Users.Add(user);
            return (_context.SaveChanges()>0);
        }

        public bool Delete(User user)
        {
            _context.Users.Remove(user);
            return (_context.SaveChanges()>0);
        }

        public User Get(int id)
        {
            return _context.Users.Include(p => p.Role).First(p=>p.Id == id);
        }

        public User Get(string username)
        {
            return _context.Users.Include(r => r.Role).First(p=>p.UserName == username);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(p=>p.Role).ToList();
        }

        public IEnumerable<User> GetAllByRole(Role role)
        {
            return _context.Users.Include(p => p.Role).Where(p => p.RoleId == role.Id);
        }

        public bool Update(User user)
        {
            _context.Users.Update(user);
            return (_context.SaveChanges()>0);
        }
    }
}
