using SomeStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using SomeStore.Data.Repositories;
using SomeStore.Data.Interfaces;

namespace SomeStore.Services
{
    public interface IShopCartService
    {
        bool IsExists(Product prod);
        int GetAmountItems(ClaimsPrincipal user);
        double GetSummaryPrice(ClaimsPrincipal user);
        bool AddItemToCart(ClaimsPrincipal user,Product product);
        bool RemoveItem(ClaimsPrincipal user, Product product);
        bool UpdateCartItem(ClaimsPrincipal user, Product product);
        bool CreateShopCart(ClaimsPrincipal user);
        IEnumerable<ShopCartItem> GetAllFromShopCart(ClaimsPrincipal user);
        ShopCart GetShopCart(ClaimsPrincipal user);
    }
    public class ShopCartService : IShopCartService
    {
        private readonly IShopCartItemRepository _itemRepo;
        private readonly IShopCartRepository _cartRepo;
        private readonly IUserRepository _userRepo;
        public ShopCartService(IShopCartItemRepository itemRepo, IShopCartRepository cartRepo, IUserRepository userRepo)
        {
            _itemRepo = itemRepo;
            _cartRepo = cartRepo;
            _userRepo = userRepo;
        }
        public bool AddItemToCart(ClaimsPrincipal user, Product product)
        {
            User us = _userRepo.Get(user.Identity.Name);
            ShopCart userCart = _cartRepo.Get(us);
            if (userCart == null)
            {
                CreateShopCart(user);
                userCart = _cartRepo.Get(us);
            }
            ShopCartItem item = new ShopCartItem()
            {
                ShopCartId = userCart.Id,
                Price = product.Price,
                ProductId = product.Id
            };
            return _itemRepo.Create(item);
        }

        public bool IsExists(Product product)
        {
            ShopCartItem check = _itemRepo.Get(product);
            return (check != null);
        }

        public bool CreateShopCart(ClaimsPrincipal user)
        {
            User us = _userRepo.Get(user.Identity.Name);
            ShopCart cart = _cartRepo.Get(us);
            if (cart == null)
            {
                cart = new ShopCart()
                {
                    UserId = us.Id
                };
                return _cartRepo.Create(cart);
            }
            else return false;
        }

        public IEnumerable<ShopCartItem> GetAllFromShopCart(ClaimsPrincipal user)
        {
            User us = _userRepo.Get(user.Identity.Name);
            if (us != null)
            {
                ShopCart cart = _cartRepo.Get(us);
                if (cart != null) return cart.ShopCartItems;
                else return null;
            }
            else return null;
        }

        public int GetAmountItems(ClaimsPrincipal user)
        {
            User us = _userRepo.Get(user.Identity.Name);
            if (us != null)
            {
                ShopCart cart = _cartRepo.Get(us);
                if (cart != null)
                {
                    return cart.ShopCartItems.Count;
                }
                else return 0;
            }
            else return 0;
        }

        public ShopCart GetShopCart(ClaimsPrincipal user)
        {
            User us = _userRepo.Get(user.Identity.Name);
            return _cartRepo.Get(us);
        }

        public double GetSummaryPrice(ClaimsPrincipal user)
        {
            User us = _userRepo.Get(user.Identity.Name);
            if (us != null)
            {
                ShopCart cart = _cartRepo.Get(us);
                if (cart != null)
                {
                    double price = 0;
                    foreach(var item in cart.ShopCartItems)
                    {
                        price += item.Product.Price;
                    }
                    return price;
                }
                else return 0;
            }
            else return 0;
        }

        public bool RemoveItem(ClaimsPrincipal user, Product product)
        {
            User us = _userRepo.Get(user.Identity.Name);
            ShopCart cart = _cartRepo.Get(us);
            if (cart != null)
            {
                ShopCartItem item = _itemRepo.GetAll().First(p=>p.ProductId == product.Id && p.ShopCartId == cart.Id);
                if (item != null) return _itemRepo.Remove(item);
                else return false;
            }
            else return false;
        }

        public bool UpdateCartItem(ClaimsPrincipal user, Product product)
        {
            User us = _userRepo.Get(user.Identity.Name);
            ShopCart cart = _cartRepo.Get(us);
            if (cart != null)
            {
                ShopCartItem item = _itemRepo.GetAll().First(p => p.ProductId == product.Id && p.ShopCartId == cart.Id);
                if (item != null) return _itemRepo.Update(item);
                else return false;
            }
            else return false;
        }
    }
}
