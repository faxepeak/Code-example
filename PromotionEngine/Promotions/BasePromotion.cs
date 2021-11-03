using Service.Exceptions;
using Service.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Promotions
{
    public abstract class BasePromotion : IPromotion
    {
        public double UsePromotion(Cart cart)
        {
            //Always validating if a cart has a item with a missing product ID.
            //Afterwards calculating the promotion for the current promotion type
            ValidateCartItems(cart);
            return CalculatePromotion(cart);
        }

        protected abstract double CalculatePromotion(Cart cart);

        internal void ValidateCartItems(Cart cart)
        {
            if (cart.Items.Select(x=> x.ProductId).Where(y => y == Guid.Empty).Any())
                throw new EmptyIdException("Empty product ID.");
        }
    }
}
