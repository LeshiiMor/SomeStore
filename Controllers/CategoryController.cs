using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Models;

namespace SomeStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository catrepo)
        {
            _categoryRepo = catrepo;
        }

        [HttpPost]
        public IActionResult AddCategory(string nameCategory)
        {
            if(String.IsNullOrEmpty(nameCategory))
            {
                return BadRequest("Пустая строка");
            }
            else
            {
                if (_categoryRepo.Create(new Category() { Name = nameCategory }))
                {
                    return Ok("Успешно !");
                }
                else return BadRequest("Ошибка при создании");
            }
        }

        [HttpGet]
        public IEnumerable<Category> GetAllCategory()
        {
            return _categoryRepo.GetAll().OrderBy(p => p.Name);
        }
    }
}
