using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Promotions
{
    public class FixedPrice : BasePromotion
    {
        private readonly List<Guid> _productIds;
        private readonly double _price;
        /// <summary>
        /// Class for the fixed price promotion.
        /// </summary>
        /// <param name="productIds">List of the fixed price products</param>
        /// <param name="price">Promotion price</param>
        public FixedPrice(List<Guid> productIds, double price)
        {
            _productIds = productIds;
            _price = price;
        }

        protected override double CalculatePromotion(Cart cart)
        {
            var totalAmount = (double)0;

            //Only getting the cart items which have more than 1 in amount.
            var activeCartItems = cart.Items.Where(x => _productIds.Contains(x.ProductId) && x.Amount > 0).ToList();

            if (activeCartItems.Count() == _productIds.Count)
            {
                //Applying the promotion while all the active cartitems has more amount than one.
                while (activeCartItems.All(x => x.Amount > 0))
                {
                    foreach (var item in _productIds)
                    {
                        activeCartItems.Single(x => x.ProductId == item).Amount -= 1;
                    }

                    totalAmount += _price;
                }
            }
            return totalAmount;
        }
    }
}
