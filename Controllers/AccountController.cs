using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.ViewModels;
using SomeStore.Services;
using SomeStore.Data.Models;
using SomeStore.Data.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SomeStore.Controllers
{
    public class AccountController : Controller
    {
        public readonly IUserService _userService;
        public readonly IAuthService _authService;
        public readonly IRoleRepository _roleRepos;
        public readonly IShopCartService _cartService;
        public readonly IOrderService _orderService;
        public AccountController(IUserService userService,IAuthService authService,IRoleRepository roleRepo, IShopCartService cartservice, IOrderService orderservice)
        {
            _userService = userService;
            _authService = authService;
            _roleRepos = roleRepo;
            _cartService = cartservice;
            _orderService = orderservice;
        }
        public IActionResult Login()
        {
            return View(new AuthViewModel());
        }

        [HttpPost]
        public IActionResult Login(AuthViewModel model)
        {
            if(String.IsNullOrEmpty(model.Password) || String.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("", "Не все поля заполнены");
                return View(model);
            }
            else
            {
                User user = _userService.GetUser(model.UserName,model.Password);
                if (user != null)
                {
                    _authService.SignInUser(user, HttpContext);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Аккаунт не найден");
                    return View(model);
                }
            }
        }

        [HttpPost]
        [Authorize]
        public async  Task<IActionResult> Logout()
        {
            await _authService.SignOut(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(string Password,string UserName,string ReentryPassword)
        {
            if (Password != ReentryPassword && ReentryPassword != null)
            {
                return BadRequest("Пароли не совпадают");
            }
            else if(_userService.GetUser(UserName,Password)!=null)
            {
                return BadRequest("Аккаунт уже существует");
            }
            else
            {
                User newUser = new User()
                {
                    UserName = UserName
                };
                Role role = _roleRepos.Get(2);
                int code = _userService.Create(newUser, Password, role);
                if (code == -1) return BadRequest("Такой логин уже существует");
                else if(code ==0) return BadRequest("Ошибка на сервере");
                _authService.SignInUser(newUser, HttpContext);
                return PartialView("_ExtendInfo");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult ClaimUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMoreInfo(string name,string surname,string patronomic,string phone)
        {
            if(String.IsNullOrEmpty(name) ||
              (String.IsNullOrEmpty(surname) ||
              (String.IsNullOrEmpty(patronomic) ||
              (String.IsNullOrEmpty(phone)))))
            {
                return BadRequest("Не все поля заполнены");
            }
            else
            {
                User user = _userService.GetUser(User.Identity.Name);
                if (user != null)
                {
                    user.Name = name;
                    user.Surname = surname;
                    user.Patronymic = patronomic;
                    user.PhoneNumber = phone;
                    _userService.Update(user);
                    return Ok();
                }
                else return BadRequest("Аккаунт не найден");
            }
            
        }


        [HttpGet]
        [Authorize]
        public IActionResult Cabinet()
        {
            PersonalInfoViewModel model = new PersonalInfoViewModel();
            if (User.Identity.IsAuthenticated)
            {
                User user = _userService.GetUser(User.Identity.Name);
                if (user != null)
                {
                    model.Name = user.Name;
                    model.Surname = user.Surname;
                    model.Patronymic = user.Patronymic;
                    model.PhoneNumber = user.PhoneNumber;
                    model.Email = user.Email;
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AuthInfo()
        {
            return PartialView("_AuthInfo");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangeLogin(string newLogin,string password)
        {
            try
            {
                if (!User.Identity.IsAuthenticated) throw new Exception("Пользователь не авторизовался");
                User user = _userService.GetUser(newLogin, password);
                if (user != null) throw new Exception("Логин занят");
                user = _userService.GetUser(User.Identity.Name, password);
                if (user == null) throw new Exception("Неверный пароль");
                user.UserName = newLogin;
                if (_userService.Update(user))
                {
                    _authService.SignOut(HttpContext);
                    _authService.SignInUser(user,HttpContext);
                    return Ok("Логин изменен");
                }
                else throw new Exception("Ошибка при обновлении данных");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(string password,string newpassword)
        {
            try
            {
                if(!User.Identity.IsAuthenticated) throw new Exception("Пользователь не авторизовался");
                if (String.IsNullOrEmpty(password) || (String.IsNullOrEmpty(newpassword))) throw new Exception("Не все поля заполнены");
                else if (password == newpassword) throw new Exception("Введен одинаковый пароль");
                User user = _userService.GetUser(User.Identity.Name, password);
                if(user==null) throw new Exception("Неверный пароль");
                if (_userService.UpdatePassword(user, newpassword)) return Ok("Успешно !");
                else throw new Exception("Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult CurrentOrderInfo()
        {
            return PartialView("_CurrentOrderInfo");
        }

        [HttpGet]
        [Authorize]
        public IActionResult HistoryOrderInfo()
        {
            ManageOrdersViewModel model = new ManageOrdersViewModel();
            User us = _userService.GetUser(User.Identity.Name);
            model.Orders = _orderService.GetAllOrder(us);
            return PartialView("_HistoryOrderInfo",model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult PersonalInfo()
        {
            PersonalInfoViewModel model = new PersonalInfoViewModel();
            if(User.Identity.IsAuthenticated)
            {
                User user = _userService.GetUser(User.Identity.Name);
                if(user!=null)
                {
                    model.Name = user.Name;
                    model.Surname = user.Surname;
                    model.Patronymic = user.Patronymic;
                    model.PhoneNumber = user.PhoneNumber;
                    model.Email = user.Email;
                }
            }
            return PartialView("_PersonalInfo",model);
        }

        [HttpPost]
        public IActionResult PersonalInfo(PersonalInfoViewModel model)
        {
           if(String.IsNullOrEmpty(model.Name) ||String.IsNullOrEmpty(model.Surname) ||String.IsNullOrEmpty(model.Patronymic))
            {
                return BadRequest("Не все поля заполнены");
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    User user = _userService.GetUser(User.Identity.Name);
                    if (user != null)
                    {
                        user.Name = model.Name;
                        user.Surname = model.Surname;
                        user.Patronymic = model.Patronymic;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Email = model.Email;
                        if (_userService.Update(user))
                        {
                            return Ok("Успешно !");
                        }
                        else return BadRequest("Ошибка при сохранении данных");
                    }
                    else return BadRequest("Пользователь не найден");
                }
                else return BadRequest("Пользователь не авторизовался");
            }
        }
    }
}
