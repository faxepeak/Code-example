using Service.Promotions;
using Service.Tests.Fakes.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.CalculatorServiceTests
{
    public static class Helpers
    {
        private static Guid _guidOne { get; set; } = new Guid("11111111-1111-1111-1111-111111111111");
        private static Guid _guidTwo { get; set; } = new Guid("22222222-2222-2222-2222-222222222222");
        private static Guid _guidThree { get; set; } = new Guid("33333333-3333-3333-3333-333333333333");
        private static Guid _guidFour { get; set; } = new Guid("44444444-4444-4444-4444-444444444444");

        public static ProductServiceFake GetBasicProductServiceFake()
        {
            var service = new ProductServiceFake();
            service.ProductPrices.Add(_guidOne, 50);
            service.ProductPrices.Add(_guidTwo, 30);
            service.ProductPrices.Add(_guidThree, 20);
            service.ProductPrices.Add(_guidFour, 15);

            return service;
        }

        public static PromotionServiceFake GetPromotionServiceFake(bool nItemsOne = false, bool nItemsTwo = false, bool fixedPriceThreeFour = false, bool percentageThree = false, ProductServiceFake productServiceFake = null, string cuponCode = "")
        {
            var service = new PromotionServiceFake();
            if (nItemsOne)
                service.ActivePromotions.Add(new NItems(_guidOne, 3, 130));
            if (nItemsTwo)
                service.ActivePromotions.Add(new NItems(_guidTwo, 2, 45));
            if (fixedPriceThreeFour)
                service.ActivePromotions.Add(new FixedPrice(new List<Guid>() { _guidThree, _guidFour }, 30));

            if (productServiceFake == null)
                productServiceFake = GetBasicProductServiceFake();

            if (percentageThree)
                service.ActivePromotions.Add(new PercentageItem(productServiceFake, _guidThree, 50));
            if (!string.IsNullOrWhiteSpace(cuponCode))
                service.CuponPromotions.Add(cuponCode, new PercentageCart(productServiceFake, 10));


            return service;
        }
    }
}
