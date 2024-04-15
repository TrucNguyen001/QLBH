using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Avatar { get; set; }

        public string? Image { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public int? QuantityBuy { get; set; }

        public decimal Price { get; set; }

        public decimal? PriceBuy { get; set; }

        public decimal? PriceReduced { get; set; }

        public Guid SupplierId { get; set; }

        public Guid ProductTypeId { get; set; }

        public int? Status {  get; set; }

        public int Hot { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
