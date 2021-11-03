using Service.Exceptions;
using Service.Models;
using Service.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Service.Tests.Promotions.NItemsTests
{
    public class UsePromotion
    {
        private Guid _guidOne { get; set; } = new Guid("11111111-1111-1111-1111-111111111111");

        [Fact]
        public void No_cart_items()
        {
            //Arrange
            var noOfItems = 3;
            var price = (double)130;
            var nItemsPromotion = new NItems(_guidOne, noOfItems, price);

            var inputCartItems = new Cart();

            var expectedOutput = 0;

            //Act
            var output = nItemsPromotion.UsePromotion(inputCartItems);

            //Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void No_product_Id()
        {
            //Arrange
            var noOfItems = 3;
            var price = (double)130;
            var nItemsPromotion = new NItems(_guidOne, noOfItems, price);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { Amount = 10 }
                }
            };

            var errorMessage = string.Empty;
            var expectedErrorMessage = "Empty product ID.";

            //Act
            try
            {
                nItemsPromotion.UsePromotion(inputCart);
            }
            catch (EmptyIdException e)
            {
                errorMessage = e.Message;
            }

            //Assert
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]
        public void No_amount()
        {
            //Arrange
            var noOfItems = 3;
            var price = (double)130;
            var nItemsPromotion = new NItems(_guidOne, noOfItems, price);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne }
                }
            };

            var expectedOutput = 0;

            //Act
            var output = nItemsPromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void Too_low_amount()
        {
            //Arrange
            var noOfItems = 3;
            var price = (double)130;
            var nItemsPromotion = new NItems(_guidOne, noOfItems, price);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 2 }
                }
            };

            var expectedOutput = 0;
            var expectedAmount = 2;

            //Act
            var output = nItemsPromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, output);
            Assert.Equal(expectedAmount, inputCart.Items.First().Amount);
        }

        [Fact]
        public void Higher_amount()
        {
            //Arrange
            var noOfItems = 3;
            var price = (double)130;
            var nItemsPromotion = new NItems(_guidOne, noOfItems, price);

            var inputCart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new CartItem() { ProductId = _guidOne, Amount = 4 }
                }
            };

            var expectedOutput = 130;
            var expectedAmount = 1;

            //Act
            var output = nItemsPromotion.UsePromotion(inputCart);

            //Assert
            Assert.Equal(expectedOutput, output);
            Assert.Equal(expectedAmount, inputCart.Items.First().Amount);
        }
    }
}
