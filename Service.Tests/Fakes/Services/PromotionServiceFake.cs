using Service.Interfaces;
using Service.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.Fakes.Services
{
    public class PromotionServiceFake : IPromotionService
    {
        public List<IPromotion> ActivePromotions { get; set; } = new List<IPromotion>();
        public Dictionary<string, IPromotion> CuponPromotions { get; set; } = new Dictionary<string, IPromotion>();
        public List<IPromotion> GetActivePromotions()
        {
            return ActivePromotions;
        }

        public IPromotion GetFromCuponCode(string cuponCode)
        {
            if (CuponPromotions.ContainsKey(cuponCode))
                return CuponPromotions[cuponCode];
            return null;
        }
    }
}
