using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}
