using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Cart
    {
        public Guid CartId { get; set; }

        public Guid ProductId {  get; set; }

        public Guid? InvoiceId { get; set; } = null;

        public Guid AccountId { get; set; }

        /// <summary>
        /// Số lượng mua
        /// </summary>
        public int QuantityPurchased { get; set; }

        public int Status { get; set; }
    }
}
