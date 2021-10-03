using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeStore.Data.Models;
using SomeStore.Data.Interfaces;
using System.Security.Claims;

namespace SomeStore.Services
{
    public interface IOrderService
    {
        bool AddOrder(ClaimsPrincipal user,Order order);
        bool AddOrder(ClaimsPrincipal user,IEnumerable<Order> orders);
        bool RemoveOrder(Order order);
        bool AcceptOrder(Order order);
        bool CancelOrder(Order order);
        Order Get(int id);
        IEnumerable<Order> GetAllOrder();
        IEnumerable<Order> GetAllOrder(Product product);
        IEnumerable<Order> GetAllOrder(User user);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _prodRepo;
        private readonly IUserRepository _userRepo;
        private readonly IShopCartItemRepository _shopItemRepo;
        public OrderService(IOrderRepository orderRepo, IProductRepository prodRepo, IUserRepository userRepo, IShopCartItemRepository shopitemrepo)
        {
            _orderRepo = orderRepo;
            _prodRepo = prodRepo;
            _userRepo = userRepo;
            _shopItemRepo = shopitemrepo;
        }
        public bool AcceptOrder(Order order)
        {
            Order ord = _orderRepo.Get(order.Id);
            order.DateChange = GetCurrentDate();
            order.Status = 1;
            return _orderRepo.Update(order);
        }

        public bool AddOrder(ClaimsPrincipal user,Order order)
        {
            User us = _userRepo.Get(user.Identity.Name);

            if (us == null) return false;

            order.Date = GetCurrentDate();
            Product prod = _prodRepo.Get(order.ProductId);
            if (prod != null)
            {
                order.FinalPrice = prod.Price * order.AmountProduct;
                order.Status = 0;
                order.UserId = us.Id;
                if (_orderRepo.Create(order))
                {
                    ShopCartItem item = _shopItemRepo.Get(prod);
                    if(item!=null) _shopItemRepo.Remove(item);
                    return true;
                }
                else return false;
            }
            else return true;
        }

        public bool AddOrder(ClaimsPrincipal user, IEnumerable<Order> orders)
        {
            int check = 0;
            foreach(var order in orders)
            {
                if (AddOrder(user, order)) check++;
            }
            if (check == orders.Count()) return true;
            else return false;
        }

        public bool CancelOrder(Order order)
        {
            Order ord = _orderRepo.Get(order.Id);
            order.DateChange = GetCurrentDate();
            order.Status = 2;
            return _orderRepo.Update(order);
        }

        public Order Get(int id)
        {
            return _orderRepo.Get(id);
        }

        public IEnumerable<Order> GetAllOrder()
        {
            return _orderRepo.GetAll();
        }

        public IEnumerable<Order> GetAllOrder(Product product)
        {
            return _orderRepo.GetAllByProduct(product);
        }

        public IEnumerable<Order> GetAllOrder(User user)
        {
            return _orderRepo.GetAllByUser(user);
        }

        public bool RemoveOrder(Order order)
        {
            return _orderRepo.Remove(order);
        }
        private string GetCurrentDate()
        {
            DateTime date = DateTime.Now;
            string dateStr = $"{date.Day}/{date.Month}/{date.Year}";
            return dateStr;
        }
    }
}
