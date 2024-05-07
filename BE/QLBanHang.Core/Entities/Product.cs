using QLBanHang.Core.MISAAttribute;
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

        [ExportFile]
        [ProppertyName("Mã sản phẩm")]
        public string ProductCode { get; set; }

        [ExportFile]
        [ProppertyName("Tên sản phẩm")]
        public string ProductName { get; set; }

        [ExportFile]
        [ProppertyName("Avatar")]
        public string Avatar { get; set; }

        [ExportFile]
        [ProppertyName("Hình ảnh")]
        public string? Image { get; set; }

        [ExportFile]
        [ProppertyName("Mô tả")]
        public string Description { get; set; }

        [ExportFile]
        [ProppertyName("Số lượng")]
        public int Quantity { get; set; }

        [ExportFile]
        [ProppertyName("Số lượng bán")]
        public int? QuantityBuy { get; set; }

        [ExportFile]
        [ProppertyName("Giá")]
        public decimal Price { get; set; }

        [ExportFile]
        [ProppertyName("Giá bán")]
        public decimal? PriceBuy { get; set; }

        [ExportFile]
        [ProppertyName("Giá giảm")]
        public decimal? PriceReduced { get; set; }

        public Guid SupplierId { get; set; }

        public Guid ProductTypeId { get; set; }

        public int? Status {  get; set; }

        [ExportFile]
        [ProppertyName("Độ hot")]
        public int Hot { get; set; }

        [ExportFile]
        [ProppertyName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
