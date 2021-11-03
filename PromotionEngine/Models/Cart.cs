using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
