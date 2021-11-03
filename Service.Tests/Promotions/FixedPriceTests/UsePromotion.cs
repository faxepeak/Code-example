using Service.Exceptions;
using Service.Models;
using Service.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Service.Tests.Promotions.FixedPriceTests
{
    public class UsePromotion
    {
        private Guid _guidOne { get; set; } = new Guid("11111111-1111-1111-1111-111111111111");
        private Guid _guidTwo { get; set; } = new Guid("22222222-2222-2222-2222-222222222222");


        [Fact]
        public void One_item()
        {
            //Arrange
            var promotionProductIds = new List<Guid>() { _guidOne, _guidTwo };
            var promotionPrice = 70;
            var fixedPricePromotion = new FixedPrice(promotionProductIds, promotionPrice);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 1 }
                }
            };

            var expectedOutput = 0;

            //Act
            var ouput = fixedPricePromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, ouput);
        }

        [Fact]
        public void No_amount_both()
        {
            //Arrange
            var promotionProductIds = new List<Guid>() { _guidOne, _guidTwo };
            var promotionPrice = 70;
            var fixedPricePromotion = new FixedPrice(promotionProductIds, promotionPrice);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne },
                    new CartItem() { ProductId = _guidTwo }
                }
            };

            var expectedOutput = 0;

            //Act
            var ouput = fixedPricePromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, ouput);
        }

        [Fact]
        public void No_amount_one()
        {
            //Arrange
            var promotionProductIds = new List<Guid>() { _guidOne, _guidTwo };
            var promotionPrice = 70;
            var fixedPricePromotion = new FixedPrice(promotionProductIds, promotionPrice);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 1 },
                    new CartItem() { ProductId = _guidTwo }
                }
            };

            var expectedOutput = 0;

            //Act
            var ouput = fixedPricePromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, ouput);
        }

        [Fact]
        public void No_Id_both()
        {
            //Arrange
            var promotionProductIds = new List<Guid>() { _guidOne, _guidTwo };
            var promotionPrice = 70;
            var fixedPricePromotion = new FixedPrice(promotionProductIds, promotionPrice);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { Amount = 1 },
                    new CartItem() { Amount = 1 }
                }
            };

            var errorMessage = string.Empty;
            var expectedErrorMessage = "Empty product ID.";

            //Act
            try
            {
                fixedPricePromotion.UsePromotion(inputCart);
            }
            catch (EmptyIdException e)
            {
                errorMessage = e.Message;
            }

            //Assert
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]
        public void No_Id_one()
        {
            //Arrange
            var promotionProductIds = new List<Guid>() { _guidOne, _guidTwo };
            var promotionPrice = 70;
            var fixedPricePromotion = new FixedPrice(promotionProductIds, promotionPrice);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 1 },
                    new CartItem() { Amount = 1 }
                }
            };

            var errorMessage = string.Empty;
            var expectedErrorMessage = "Empty product ID.";

            //Act
            try
            {
                fixedPricePromotion.UsePromotion(inputCart);
            }
            catch (EmptyIdException e)
            {
                errorMessage = e.Message;
            }

            //Assert
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]
        public void Extra_amount_for_one()
        {
            //Arrange
            var promotionProductIds = new List<Guid>() { _guidOne, _guidTwo };
            var promotionPrice = 70;
            var fixedPricePromotion = new FixedPrice(promotionProductIds, promotionPrice);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 2 },
                    new CartItem() { ProductId = _guidTwo, Amount = 1 }
                }
            };

            var expectedOutput = 70;
            var expectedAmountFor1 = 1;

            //Act
            var ouput = fixedPricePromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, ouput);
            Assert.Equal(expectedAmountFor1, inputCart.Items.Single(x => x.ProductId == _guidOne).Amount);
        }

        [Fact]
        public void Extra_amount_for_two()
        {
            //Arrange
            var promotionProductIds = new List<Guid>() { _guidOne, _guidTwo };
            var promotionPrice = 70;
            var fixedPricePromotion = new FixedPrice(promotionProductIds, promotionPrice);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                new CartItem() { ProductId = _guidOne, Amount = 2 },
                new CartItem() { ProductId = _guidTwo, Amount = 2 }
                }
            };

            var expectedOutput = 140;
            var expectedAmountFor1 = 0;
            var expectedAmountFor2 = 0;

            //Act
            var ouput = fixedPricePromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, ouput);
            Assert.Equal(expectedAmountFor1, inputCart.Items.Single(x => x.ProductId == _guidOne).Amount);
            Assert.Equal(expectedAmountFor2, inputCart.Items.Single(x => x.ProductId == _guidTwo).Amount);
        }
    }
}
