using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IPromotion
    {
        public double UsePromotion(Cart cart);
    }
}
