using Service.Interfaces.Services;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Promotions
{
    public class PercentageItem : BasePromotion
    {
        private readonly IProductService _productService;
        private readonly Guid _productId;
        private readonly double _percentageOff;

        public PercentageItem(IProductService productService, Guid productId, double percentageOff)
        {
            _productService = productService;
            _productId = productId;
            _percentageOff = percentageOff;
        }

        protected override double CalculatePromotion(Cart cart)
        {
            var totalOutput = (double)0;
            var currentItem = cart.Items.SingleOrDefault(x => x.ProductId == _productId);

            if (currentItem != null)
            {
                var unitPrice = _productService.GetProductPriceById(currentItem.ProductId);

                //Calculating the price for the amount of products
                var priceTotal = unitPrice * currentItem.Amount;

                //Calculating the new price after the percentages has been removed.
                totalOutput = priceTotal - ((priceTotal * _percentageOff) / 100);

                currentItem.Amount = 0;
            }

            return totalOutput;
        }
    }
}
