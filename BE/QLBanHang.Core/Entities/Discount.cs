using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Discount
    {
        public Guid DiscountId { get; set; }

        public string DiscountCode { get; set; }

        public string DiscountName {  get; set; }

        /// <summary>
        /// Số tiền giảm
        /// </summary>
        public string ReducedAmount { get; set; }

        /// <summary>
        /// Số lần nhập
        /// </summary>
        public int InputNumber { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Đơn tối thiểu
        /// </summary>
        public decimal Minimum {  get; set; }

        public int Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
