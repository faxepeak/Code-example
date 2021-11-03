using Service.Interfaces.Services;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Promotions
{
    public class PercentageCart : BasePromotion
    {
        private readonly IProductService _productService;
        private readonly double _percentageOff;

        public PercentageCart(IProductService productService, double percentageOff)
        {
            _productService = productService;
            _percentageOff = percentageOff;
        }

        protected override double CalculatePromotion(Cart cart)
        {
            var totalOutput = (double)0;

            foreach (var cartItem in cart.Items)
            {
                var unitPrice = _productService.GetProductPriceById(cartItem.ProductId);

                //Calculating the price for the amount of products
                var priceTotal = unitPrice * cartItem.Amount;

                //Calculating the new price after the percentages has been removed.
                totalOutput += priceTotal - ((priceTotal * _percentageOff) / 100);

                cartItem.Amount = 0;
            }

            return totalOutput;
        }
    }
}
