using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.ViewModels;
using SomeStore.Services;
using SomeStore.Data.Interfaces;
using SomeStore.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SomeStore.Controllers
{
    [Authorize(Roles ="admin,manager")]
    public class ManageController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleRepository _roleRepo;
        private readonly IProductRepository _prodRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IOrderService _orderService;
        public ManageController(IUserService userservice,IRoleRepository roleRepo,IProductRepository prodrepo,ICategoryRepository catrepo,IOrderService orderservice)
        {
            _userService = userservice;
            _roleRepo = roleRepo;
            _prodRepo = prodrepo;
            _categoryRepo = catrepo;
            _orderService = orderservice;
        }
        public IActionResult Index()
        {
            AdminAccountsViewModel model = new AdminAccountsViewModel();
            model.Users = _userService.GetAll();
            model.Roles = _roleRepo.GetAll();
            model.RolesList = new SelectList(_roleRepo.GetAll(), "Id", "Name");
            return View(model);
        }

        [HttpGet]
        public IActionResult Accounts()
        {
            AdminAccountsViewModel model = new AdminAccountsViewModel();
            model.Users = _userService.GetAll();
            model.Roles = _roleRepo.GetAll();
            model.RolesList = new SelectList(_roleRepo.GetAll(),"Id","Name");
            return PartialView("_Accounts",model);
        }

        [HttpGet]
        public IActionResult Products()
        {
            ManageProductsViewModel model = new ManageProductsViewModel();
            model.Products = _prodRepo.GetAll();
            model.Categories = new SelectList(_categoryRepo.GetAll(),"Id","Name");
            return PartialView("_Products",model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Orders()
        {
            ManageOrdersViewModel model = new ManageOrdersViewModel();
            model.Orders = _orderService.GetAllOrder().OrderBy(p=>p.Status);
            return PartialView("_Orders",model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<Order> GetOrder(int id)
        {
            Order order = _orderService.Get(id);
            if (order != null) return Ok(order);
            else return NotFound("Заказ не найден");
        }

        [Authorize]
        [HttpPost]
        public IActionResult CompleteOrder(int id)
        {
            Order order = _orderService.Get(id);
            if (order != null)
            {
                if (_orderService.AcceptOrder(order)) return Ok();
                else return BadRequest();
            }
            else return NotFound("Заказ не найден");
        }

        [Authorize]
        [HttpPost]
        public IActionResult CancelOrder(int id)
        {
            Order order = _orderService.Get(id);
            if (order != null)
            {
                if (_orderService.CancelOrder(order)) return Ok();
                else return BadRequest();
            }
            else return NotFound("Заказ не найден");
        }

        [HttpGet]
        public User GetUser(int id)
        {
            User user = _userService.GetUser(id);
            return user;
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            if (id == 0) return BadRequest("Неправильный id аккаунта");
            else if (_userService.Remove(id)) return Ok("Аккаунт удален");
            else return BadRequest("Ошибка при удалении");
        }

        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            User updateUser = _userService.GetUser(user.Id);
            if(updateUser!=null)
            {
                updateUser.Name = user.Name;
                updateUser.Surname = user.Surname;
                updateUser.Patronymic = user.Patronymic;
                updateUser.PhoneNumber = user.PhoneNumber;
                updateUser.Email = user.Email;
                updateUser.RoleId = user.RoleId;
                if (_userService.Update(updateUser)) return Ok("Успешно !");
                else return BadRequest("Ошибка при обновлении данных");
            }
            else return BadRequest("Аккаунт не найден");
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetFiltrusers(string? username , int? idRole)
        {
            IEnumerable<User> list; ;
            if (!String.IsNullOrEmpty(username) && idRole != 0)
            {
                list = _userService.GetAll().Where(p => p.UserName == username && p.RoleId == idRole);
            }
            else if (idRole != 0 && String.IsNullOrEmpty(username))
            {
                list = _userService.GetAll().Where(p =>p.RoleId == idRole);
            }
            else if (!String.IsNullOrEmpty(username) && idRole == 0)
            {
                list = _userService.GetAll().Where(p => p.UserName == username);
            }
            else return Ok(_userService.GetAll());

            if (list.Count() != 0) return Ok(list);
            else return NotFound("Данных нет");
        }
    }
}
