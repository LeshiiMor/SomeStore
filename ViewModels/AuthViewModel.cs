using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeStore.ViewModels
{
    public class AuthViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? ReentryPassword { get; set; }
    }
}
