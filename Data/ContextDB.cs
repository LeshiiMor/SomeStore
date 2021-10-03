using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SomeStore.Data.Models;

namespace SomeStore.Data
{
    public class ContextDB : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ShopCart> ShopCarts { get; set; }
        public DbSet<ShopCartItem> ShopCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ContextDB(DbContextOptions<ContextDB> options): base(options)
        {
            InitValues();
        }
        private void InitValues()
        {
            if (Roles.Count() == 0)
            {
                Role adminRole = new Role() {
                    Id=1,
                    Name="admin"
                };
                Role clientRole = new Role()
                {
                    Id = 2,
                    Name = "client"
                };
                Role managerRole = new Role()
                {
                    Id = 3,
                    Name = "manager"
                };
                Roles.AddRange(adminRole,clientRole,managerRole);
            }
            else if(Users.Count()==0)
            {
                
            }
            SaveChanges();
        }
    }
}
