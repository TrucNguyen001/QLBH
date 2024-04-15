using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }

        public string InvoiceCode { get; set; }

        public string? UserName { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? DiscountId { get; set; }

        public decimal? Total {  get; set; }

        public int? StatusInvoice { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
