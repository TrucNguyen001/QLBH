using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    public class UpdateQuantityProduct
    {
        public Guid ProductId { get; set; }

        public int QuantityPurchased { get; set; }
    }
}
