using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeStore.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double FinalPrice{get;set;}
        public string Date { get; set; }
        public string DateChange { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int AmountProduct { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
    }
}
