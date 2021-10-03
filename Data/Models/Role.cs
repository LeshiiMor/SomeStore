﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SomeStore.Data.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Укажите название роли")]
        public string Name { get; set; }
    }
}
