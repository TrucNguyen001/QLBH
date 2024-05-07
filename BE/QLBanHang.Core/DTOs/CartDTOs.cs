using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    public class CartDTOs : Cart
    {
        public string ProductCode { get; set; }

        public string InvoiceCode { get; set; }

        public Guid? InvoiceId { get; set; }

        public string? UserName { get; set; }

        public Guid? DiscountId { get; set; }

        public decimal? Total { get; set; }

        public int? StatusInvoice { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Avatar { get; set; }

        public string ProductName { get; set; }

        public decimal? PriceBuy { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public int? pay {  get; set; }

        public int IsSuccess { get; set; }

        public Guid ProductTypeId { get; set; }
    }
}
