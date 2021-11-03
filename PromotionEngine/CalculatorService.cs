using Service.Exceptions;
using Service.Interfaces.Services;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine
{
    public class CalculatorService
    {

        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;
        public CalculatorService(IProductService productService, IPromotionService promotionService)
        {
            _productService = productService;
            _promotionService = promotionService;
        }

        public double CalculatePrice(Cart cart, string cuponCode)
        {
            var total = (double)0;

            //Calculating the price when applying each avaible promotion.
            foreach (var promtion in _promotionService.GetActivePromotions())
            {
                total += promtion.UsePromotion(cart);
            }

            //Applying the cupon code promotion after active promtions has been applied.
            if (!string.IsNullOrWhiteSpace(cuponCode))
            {
                var cuponPromotion = _promotionService.GetFromCuponCode(cuponCode);

                if (cuponPromotion == null)
                    throw new Exception("Invalid cupon code");

                total += cuponPromotion.UsePromotion(cart);
            }

            //Final calculation.
            foreach (var cartItem in cart.Items)
            {
                //Calculating the totalt price for the current item, using the prices of the unit listede in eg. a database.
                var unitPrice = GetUnitPrice(cartItem.ProductId);
                var itemTotal = unitPrice * cartItem.Amount;
                total += itemTotal;
            }

            return total;
        }

        internal double GetUnitPrice(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new EmptyIdException("Empty product ID.");

            return _productService.GetProductPriceById(productId);
        }
    }
}
