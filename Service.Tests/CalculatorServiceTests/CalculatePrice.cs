using PromotionEngine;
using Service.Exceptions;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.CalculatorServiceTests
{
    public class CalculatePrice
    {
        private static Guid _guidOne { get; set; } = new Guid("11111111-1111-1111-1111-111111111111");
        private static Guid _guidThree { get; set; } = new Guid("33333333-3333-3333-3333-333333333333");
        private static Guid _guidFour { get; set; } = new Guid("44444444-4444-4444-4444-444444444444");

        #region "No active promotions"

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", 50)]
        [InlineData("22222222-2222-2222-2222-222222222222", 30)]
        [InlineData("33333333-3333-3333-3333-333333333333", 20)]
        [InlineData("44444444-4444-4444-4444-444444444444", 15)]
        public void No_promotion_single_product(string productId, double expectedOutputPrice)
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake();
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = new Guid(productId), Amount = 1 }
                }
            };
            var inputCuponCode = string.Empty;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222", 80)]
        [InlineData("11111111-1111-1111-1111-111111111111", "33333333-3333-3333-3333-333333333333", 70)]
        [InlineData("22222222-2222-2222-2222-222222222222", "33333333-3333-3333-3333-333333333333", 50)]
        [InlineData("33333333-3333-3333-3333-333333333333", "44444444-4444-4444-4444-444444444444", 35)]

        public void No_promotion_two_products(string productIdOne, string productIdTwo, double expectedOutputPrice)
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake();
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = new Guid(productIdOne), Amount = 1 },
                    new CartItem() { ProductId = new Guid(productIdTwo), Amount = 1 }
                }
            };
            var inputCuponCode = string.Empty;


            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        #endregion

        #region "Single promotions"

        [Fact]
        public void NItems_single_promotion()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 3 },
                }
            };
            var inputCuponCode = string.Empty;


            var expectedOutputPrice = 130;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void Fixed_price_single_promotion()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(fixedPriceThreeFour: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidThree, Amount = 1 },
                    new CartItem() { ProductId = _guidFour, Amount = 1 }
                }
            };
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 30;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void Percentage_single_promotion()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(percentageThree: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidThree, Amount = 1 },
                }
            };
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 10;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void Cupon_percentage_single_promotion()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(cuponCode: "10%Off");
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 1 },
                    new CartItem() { ProductId = _guidThree, Amount = 1 },
                }
            };
            var inputCuponCode = "10%Off";

            var expectedOutputPrice = 63;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        #endregion

        #region "Multipel promotions"

        [Fact]
        public void NItems_and_fixed_price()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true, fixedPriceThreeFour: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 3 },
                    new CartItem() { ProductId = _guidThree, Amount = 1 },
                    new CartItem() { ProductId = _guidFour, Amount = 1 },
                }
            };
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 160;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void NItems_and_percentage()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true, percentageThree: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 3 },
                    new CartItem() { ProductId = _guidThree, Amount = 10 },                    
                }
            };
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 230;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }
        
        [Fact]
        public void Fixed_price_and_percentage()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(fixedPriceThreeFour: true, percentageThree: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidThree, Amount = 2 },
                    new CartItem() { ProductId = _guidFour, Amount = 1 },
                }
            };
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 40;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void NItems_fixed_price_percentage()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true ,fixedPriceThreeFour: true, percentageThree: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 3},
                    new CartItem() { ProductId = _guidThree, Amount = 2 },
                    new CartItem() { ProductId = _guidFour, Amount = 1 },
                }
            };
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 170;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void NItems_cupon_percentage()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true,cuponCode: "10%Off");
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 4 },
                    new CartItem() { ProductId = _guidFour, Amount = 1 },
                }
            };
            var inputCuponCode = "10%Off";

            var expectedOutputPrice = 188.5;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void Fixed_price_cupon_percentage()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(fixedPriceThreeFour: true, cuponCode: "10%Off");
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidThree, Amount = 5 },
                    new CartItem() { ProductId = _guidFour, Amount = 2 },
                }
            };
            var inputCuponCode = "10%Off";

            var expectedOutputPrice = 114;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        #endregion

        #region "Outliers"

        [Fact]
        public void Empty_cart_list()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true, nItemsTwo: true, fixedPriceThreeFour: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart();
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 0;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void Cart_item_missing_amount()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true, nItemsTwo: true, fixedPriceThreeFour: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                new CartItem() { ProductId = _guidOne }
                }
            };
            var inputCuponCode = string.Empty;

            var expectedOutputPrice = 0;

            //Act
            var output = calculator.CalculatePrice(inputCart, inputCuponCode);

            //Asset
            Assert.Equal(expectedOutputPrice, output);
        }

        [Fact]
        public void Cart_item_missing_product_Id()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true, nItemsTwo: true, fixedPriceThreeFour: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                   new CartItem() { Amount = 1 }
                }
            };
            var inputCuponCode = string.Empty;

            var expectedErrorMessage = "Empty product ID.";
            var errorMessage = string.Empty;

            //Act
            try
            {
                var output = calculator.CalculatePrice(inputCart, inputCuponCode);
            }
            catch (EmptyIdException e)
            {
                errorMessage = e.Message;
            }

            //Asset
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]
        public void Invalid_cupon_code()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var promotionServiceStub = Helpers.GetPromotionServiceFake(nItemsOne: true, nItemsTwo: true, fixedPriceThreeFour: true);
            var calculator = new CalculatorService(productServiceStub, promotionServiceStub);

            var inputCart = new Cart();
            var inputCuponCode = "test code";

            var expectedErrorMessage = "Invalid cupon code";
            var errorMessage = string.Empty;

            //Act
            try
            {
                var output = calculator.CalculatePrice(inputCart, inputCuponCode);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            //Asset
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        #endregion
    }
}
