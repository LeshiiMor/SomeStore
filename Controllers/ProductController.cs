using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using SomeStore.ViewModels;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SomeStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _prodRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IWebHostEnvironment _webHost;
        public ProductController(IWebHostEnvironment webhost, IProductRepository repo, ICategoryRepository categoryRepo)
        {
            _prodRepo = repo;
            _categoryRepo = categoryRepo;
            _webHost = webhost;
        }
        public IActionResult Index()
        {
            IndexProductViewModel model = new IndexProductViewModel()
            {
                Products = _prodRepo.GetAll(),
                Categories = new SelectList(_categoryRepo.GetAll().OrderBy(p => p.Name), "Id", "Name")
            };
            return View(model);
        }
        public IActionResult ProductCard(int id)
        {
            return View(_prodRepo.Get(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateProductViewModel model = new CreateProductViewModel();
            model.Categories = new SelectList(_categoryRepo.GetAll().OrderBy(p => p.Name), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            bool check = false;
            if (model.NameProduct != null)
            {
                foreach (var symbol in model.NameProduct) if (Char.IsNumber(symbol)) check = true;
            }
            if (check)
            {
                ModelState.AddModelError("NameProduct", "Цифры не должны быть в названии");
            }
            if (model.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Неверная категория");
            }
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Name = model.NameProduct,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId
                };
                if (model.ProductPhoto != null)
                {
                    string path = "/Content/Product/" + model.ProductPhoto.FileName;
                    using (var fileStream = new FileStream(_webHost.WebRootPath + path, FileMode.Create))
                    {
                        await model.ProductPhoto.CopyToAsync(fileStream);
                        product.ImageURL = path;
                    }
                }
                _prodRepo.Create(product);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                model.Categories = new SelectList(_categoryRepo.GetAll().OrderBy(p => p.Name), "Id", "Name");
                return View(model);
            }
        }

        [HttpGet]
        public IEnumerable<Product> GetFiltrProduct(string? name, int? category)
        {
            if (!String.IsNullOrEmpty(name) && category != 0)
            {
                return _prodRepo.GetAll().Where(p => p.Name.Contains(name) && p.CategoryId == category);
            }
            else if (!String.IsNullOrEmpty(name) && category == 0)
            {
                return _prodRepo.GetAll().Where(p => p.Name.Contains(name));
            }
            else if (category != 0 && String.IsNullOrEmpty(name))
            {
                return _prodRepo.GetAll().Where(p => p.CategoryId == category);
            }
            return _prodRepo.GetAll();
        }

        [HttpGet]
        public ActionResult<Product> GetProduct(int id)
        {
            if (id == 0) return BadRequest("Неправильный id");
            else
            {
                Product product = _prodRepo.Get(id);
                if (product != null) return Ok(product);
                else return NotFound("Данных нет");
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                Product product = _prodRepo.Get(id);
                if (product != null)
                {
                    if (_prodRepo.Delete(product)) return Ok("Успешно");
                    else return BadRequest("Ошибка при удалении");
                }
                else return BadRequest("Продукт не найден");
            }
            else return BadRequest("Неправильный id");
        }

        [HttpPost]
        public IActionResult Update(Product prod)
        {
            if(prod!=null)
            {
                if (_prodRepo.Update(prod)) return Ok("Успешно !");
                else return BadRequest("Ошибка при обновлении данных");
            }    
            return BadRequest("Данных нет");
        }
    }
}
