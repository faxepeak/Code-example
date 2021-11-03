using Service.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.Fakes.Services
{
    public class ProductServiceFake : IProductService
    {
        public Dictionary<Guid, double> ProductPrices { get; set; } = new Dictionary<Guid, double>();
        public double GetProductPriceById(Guid id)
        {
            return ProductPrices[id];
        }
    }
}
