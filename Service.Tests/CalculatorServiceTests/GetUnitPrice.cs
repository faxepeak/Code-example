using PromotionEngine;
using Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Service.Tests.CalculatorServiceTests
{
    public class GetUnitPrice
    {
        [Theory]
        [InlineData("11111111-1111-1111-1111-111111111111", 50)]
        [InlineData("22222222-2222-2222-2222-222222222222", 30)]
        [InlineData("33333333-3333-3333-3333-333333333333", 20)]
        [InlineData("44444444-4444-4444-4444-444444444444", 15)]
        public void Get_price(string guidString, double expectedOutput)
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var calculator = new CalculatorService(productServiceStub, null);

            var inputProductId = new Guid(guidString);

            //Act
            var output = calculator.GetUnitPrice(inputProductId);

            //Assert
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void With_empty_product_ID()
        {
            //Arrange
            var productServiceStub = Helpers.GetBasicProductServiceFake();
            var calculator = new CalculatorService(productServiceStub, null);

            var inputProductId = Guid.Empty;

            var expectedErrorMessage = "Empty product ID.";
            var errorMessage = string.Empty;

            //Act
            try
            {
                var output = calculator.GetUnitPrice(inputProductId);
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
