using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    public class InvoiceDiscountDTOs : Invoice
    {
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
        public decimal Minimum { get; set; }

        public int Status { get; set; }
    }
}
