using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Promotions
{
    public class NItems : BasePromotion
    {
        private readonly Guid _productId;
        private readonly int _noOfItems;
        private readonly double _price;
        /// <summary>
        /// Class for the 'n' items promotion.
        /// </summary>
        /// <param name="productId">What ID the promotion should be active on.</param>
        /// <param name="noOfItems">No of items the promotion is active for.</param>
        /// <param name="price">The new price for the promotion.</param>
        public NItems(Guid productId, int noOfItems, double price)
        {
            _productId = productId;
            _noOfItems = noOfItems;
            _price = price;
        }

        protected override double CalculatePromotion(Cart cart)
        {
            var currentCartItem = cart.Items.SingleOrDefault(x => x.ProductId == _productId);
            var outputTotal = (double)0;

            if (currentCartItem != null)
            {
                //Applying the promotion while the active cart items has the required number of items.
                while (currentCartItem.Amount >= _noOfItems)
                {
                    outputTotal += _price;
                    currentCartItem.Amount -= _noOfItems;
                }
            }

            return outputTotal;
        }
    }
}
