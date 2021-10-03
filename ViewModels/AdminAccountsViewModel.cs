using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomeStore.Data.Models;

namespace SomeStore.ViewModels
{
    public class AdminAccountsViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public SelectList RolesList { get; set; }
    }
}
