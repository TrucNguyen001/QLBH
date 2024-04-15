using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface IDiscountRepository : IBaseRepository<Discount>
    {
        /// <summary>
        /// Lấy thông tin mã giảm giá bằng mã
        /// </summary>
        /// <param name="discountCode"></param>
        /// <returns></returns>
        Discount GetByDiscountCode(string discountCode);

        /// <summary>
        /// Cập nhật lại số lần nhập
        /// </summary>
        /// <param name="discountId">id giảm giá</param>
        /// <returns></returns>
        int UpdateInputNumber(Guid discountId, int number);
    }
}
