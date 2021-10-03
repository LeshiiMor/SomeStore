using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;
using SomeStore.Data.Interfaces;
using System.Security.Cryptography;

namespace SomeStore.Services
{
    public interface IUserService
    {
        int Create(User user,string password, Role role);
        bool Create(User user);
        bool Update(User user);
        bool Remove(User user);
        bool Remove(int id);
        bool UpdatePassword(User user,string newPassword);
        Role GetUserRole(User user);
        bool ChangeUserRole(User user,Role switchRole);
        User GetUser(string username,string password);
        User GetUser(string usernamae);
        User GetUser(int id);
        IEnumerable<User> GetAll();
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository usRepo)
        {
            _userRepo = usRepo;
        }
        public int Create(User user, string password,Role role)
        {
            User check = _userRepo.GetAll().FirstOrDefault(p=>p.UserName==user.UserName);
            if (check != null) return -1;
            user.Password = HashPassword(password);
            user.RoleId = role.Id;
            if (_userRepo.Create(user)) return 1;
            else return 0;
        }

        public bool Create(User user)
        {
            return false;
        }

        public bool Remove(User user)
        {
            return _userRepo.Delete(user);
        }
        public bool Remove(int id)
        {
            User deletedUser = _userRepo.Get(id);
            if (deletedUser == null) return false;
            else return _userRepo.Delete(deletedUser);
        }

        public bool Update(User user)
        {
            return _userRepo.Update(user);
        }

        public bool UpdatePassword(User user, string newPassword)
        {
            user.Password = HashPassword(newPassword);
            return _userRepo.Update(user);
        }
        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return AreHashesEqual(buffer3, buffer4);
        }
        private bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }

        public Role GetUserRole(User user)
        {
            throw new NotImplementedException();
        }

        public bool ChangeUserRole(User user, Role switchRole)
        {
            user.RoleId = switchRole.Id;
            return _userRepo.Update(user);
        }

        public User GetUser(string username, string password)
        {
            foreach(var user in _userRepo.GetAll().ToList())
            {
                if (user.UserName == username && VerifyHashedPassword(user.Password, password)) return user;
            }
            return null;
        }
        public User GetUser(string username)
        {
            return _userRepo.GetAll().FirstOrDefault(p => p.UserName == username);
        }
        public User GetUser(int id)
        {
            return _userRepo.GetAll().FirstOrDefault(p => p.Id == id);
        }
        public IEnumerable<User> GetAll()
        {
            return _userRepo.GetAll(); 
        }
    }
}
