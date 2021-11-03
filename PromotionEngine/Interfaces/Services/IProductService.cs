using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces.Services
{
    public interface IProductService
    {
        public double GetProductPriceById(Guid id);
    }
}
