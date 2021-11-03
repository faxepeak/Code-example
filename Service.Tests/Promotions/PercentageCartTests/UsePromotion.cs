using Service.Exceptions;
using Service.Models;
using Service.Promotions;
using Service.Tests.Fakes.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.Promotions.PercentageCartTests
{
    public class UsePromotion
    {
        private static Guid _guidOne { get; set; } = new Guid("11111111-1111-1111-1111-111111111111");
        private static Guid _guidThree { get; set; } = new Guid("33333333-3333-3333-3333-333333333333");

        [Fact]
        public void One_item()
        {
            //Arrange
            var productServiceStub = new ProductServiceFake();
            productServiceStub.ProductPrices.Add(_guidOne, 50);
            var percentagePromotion = new PercentageCart(productServiceStub, 10);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() {ProductId = _guidOne, Amount = 1}
                }
            };

            var expectedOutput = 45;

            //Act
            var output = percentagePromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void Two_items()
        {
            //Arrange
            var productServiceStub = new ProductServiceFake();
            productServiceStub.ProductPrices.Add(_guidOne, 50);
            productServiceStub.ProductPrices.Add(_guidThree, 20);
            var percentagePromotion = new PercentageCart(productServiceStub, 10);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() {ProductId = _guidOne, Amount = 2},
                    new CartItem() {ProductId = _guidThree, Amount = 4}

                }
            };

            var expectedOutput = 162;

            //Act
            var output = percentagePromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void No_product_Id()
        {
            //Arrange
            var productServiceStub = new ProductServiceFake();
            var percentagePromotion = new PercentageCart(productServiceStub, 10);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { Amount = 1}
                }
            };

            var errorMessage = string.Empty;
            var expectedErrorMessage = "Empty product ID.";

            //Act
            try
            {
                percentagePromotion.UsePromotion(inputCart);
            }
            catch (EmptyIdException e)
            {
                errorMessage = e.Message;
            }

            //Assert
            Assert.Equal(expectedErrorMessage, errorMessage);
        }
    }
}
