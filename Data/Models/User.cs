using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SomeStore.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Укажите логин")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage ="Укажите пароль")]
        public string Password { get; set; }
        
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage ="Неправильно указан email")]
        public string Email { get; set; }
        public Role Role { get; set; }

        public int RoleId { get; set; }
    }
}
