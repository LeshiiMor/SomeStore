using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SomeStore.Data;
using SomeStore.Data.Models;
using SomeStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Interfaces;

namespace SomeStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _prodRepo;

        public HomeController(ILogger<HomeController> logger, IProductRepository repo)
        {
            _logger = logger;
            _prodRepo = repo;
        }

        public IActionResult Index()
        {
            return View(_prodRepo.GetAllFavourite());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductCard(int id)
        {
            Product product = _prodRepo.Get(id);
            return View(product);
        }
    }
}
