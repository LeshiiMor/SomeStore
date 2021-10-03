using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;

namespace SomeStore.ViewModels
{
    public class ManageOrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
    }
}
