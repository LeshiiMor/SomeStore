using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SomeStore.Data;
using SomeStore.Data.Models;
using SomeStore.Data.Interfaces;
using SomeStore.Services;
using SomeStore.ViewModels;
using System.Text.Json;

namespace SomeStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _prodRepo;
        private readonly IShopCartService _cartService;
        private readonly IOrderService _orderService;
        public CartController(IProductRepository prodrepo, IShopCartService cartservice,IOrderService orderservice)
        {
            _prodRepo = prodrepo;
            _cartService = cartservice;
            _orderService = orderservice;
        }
        [Authorize]
        [Route("Cart")]
        [HttpGet]
        public IActionResult Index()
        {
            ShopCartViewModel model = new ShopCartViewModel();
            ShopCart cart = _cartService.GetShopCart(User);
            if(cart!=null) model.ShopCart = cart;
            if (model.ShopCart.ShopCartItems.Count >0)
            {
                foreach (var prod in model.ShopCart.ShopCartItems)
                {
                    model.FinalPrice += prod.Price;
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddProduct(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    Product addProd = _prodRepo.Get(id);
                    if (addProd != null)
                    {
                        if (_cartService.IsExists(addProd)) throw new Exception("Продукт уже в корзине");
                        if (_cartService.AddItemToCart(User, addProd)) return Ok();
                        else throw new Exception("Ошибка при добавлении продукта");
                    }
                    else throw new Exception("Продукт не найден");
                }
                else throw new Exception("пользователь не авторизовался");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteItem(int prodId)
        {
            try
            {
                if(User.Identity.IsAuthenticated)
                {
                    Product deleteProd = _prodRepo.Get(prodId);
                    if (deleteProd != null)
                    {
                        if (_cartService.RemoveItem(User, deleteProd))
                        {
                            return Ok(_cartService.GetSummaryPrice(User));
                        }
                        else throw new Exception("Ошибка при удалении продукта");
                    }
                    else throw new Exception("Продукт не найден");
                }
                else throw new Exception("пользователь не авторизовался");

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAmountItems()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return Ok(_cartService.GetAmountItems(User));
                }
                else throw new Exception("пользователь не авторизовался");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult MakeOrder(List<string> orderMap)
        {
            List<Order> orders = new List<Order>();
            foreach (var order in orderMap) orders.Add(JsonSerializer.Deserialize<Order>(order));
            if (_orderService.AddOrder(User, orders)) return Ok();
            else return BadRequest("Не все заказы удалось оформить");
        }
    }
}
