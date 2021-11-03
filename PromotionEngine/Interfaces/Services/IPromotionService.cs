using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces.Services
{
    public interface IPromotionService
    {
        public List<IPromotion> GetActivePromotions();
        public IPromotion GetFromCuponCode(string cuponCode);
    }
}
