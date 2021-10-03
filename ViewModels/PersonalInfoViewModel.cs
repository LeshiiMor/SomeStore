using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SomeStore.ViewModels
{
    public class PersonalInfoViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        
        [EmailAddress(ErrorMessage = "Некорректный email адрес")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
