using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Service
{
    public class DiscountService : BaseService<Discount>, IDiscountService
    {
        IDiscountRepository _discountRepository;
        public DiscountService(IDiscountRepository discountRepository) : base(discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public string GetDiscountCode()
        {
            string kyTu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*<>,.?/";
            Random random = new Random();
            char[] mangKyTu = new char[7];
            for (int i = 0; i < 7; i++)
            {
                mangKyTu[i] = kyTu[random.Next(kyTu.Length)];
            }
            return new string(mangKyTu);
        }

        public int UpdateInputNumber(Guid discountId, int number)
        {
            if(discountId != null)
            {
                _discountRepository.UpdateInputNumber(discountId, number);
            }
            return 1;
        }
    }
}
