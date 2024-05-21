using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    public class ProductForUserDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceBuy { get; set; }
        public string? Avatar {  get; set; }
        public int TotalQuantityPurchased { get; set; }
    }
}
