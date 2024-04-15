using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Services
{
    public interface IDiscountService : IBaseService<Discount>
    {
        /// <summary>
        /// Hàm lấy ra mã 
        /// </summary>
        /// <returns>
        /// Trả về mã
        /// </returns>
        /// CreatedBy: NVTruc(6/4/2024)
        public string GetDiscountCode();

        /// <summary>
        /// Cập nhật lại số lần nhập
        /// </summary>
        /// <param name="discountId">id mã</param>
        /// <param name="number">số lần nhập</param>
        /// <returns>1: thành công</returns>
        public int UpdateInputNumber(Guid discountId, int number);
    }
}
