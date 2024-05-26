using QLBanHang.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    public class ProductImportDTO
    {
        [ProppertyName("Id sản phẩm")]
        public string ProductId {  get; set; }

        [ProppertyName("Mã sản phẩm")]
        public string ProductCode { get; set; }

        [ProppertyName("Tên sản phẩm")]
        public string ProductName { get; set; }

        [ProppertyName("Avatar")]
        public string Avatar { get; set; }

        [ProppertyName("Số lượng")]
        public int Quantity { get; set; }

        [ProppertyName("Giá giảm")]
        public decimal PriceReduced { get; set; }

        [ProppertyName("Giá bán")]
        public decimal Price { get; set; }

        [ProppertyName("Độ hot")]
        public int Hot {  get; set; }

        public bool IsImported { get; set; } = false;

        public List<string>? ImportInvalidErrors { get; set; } = new List<string>();

        public string ProductTypeName { get; set; }

        public string SupplierName { get; set; }

    }
}
