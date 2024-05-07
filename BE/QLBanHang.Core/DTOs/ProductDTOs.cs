using QLBanHang.Core.Entities;
using QLBanHang.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    public class ProductDTOs
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

        public int? Status { get; set; }

        [ExportFile]
        [ProppertyName("Độ hot")]
        public int Hot { get; set; }

        [ExportFile]
        [ProppertyName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public Guid ConfigurationId { get; set; }

        [ExportFile]
        [ProppertyName("Màn hình")]
        public string? Screen { get; set; }

        [ExportFile]
        [ProppertyName("Độ phân giải")]
        /// <summary>
        /// Độ phân giải
        /// </summary>
        public string? Resolution { get; set; }

        [ExportFile]
        [ProppertyName("Công nghệ màn hình")]
        /// <summary>
        /// Công nghệ màn hình
        /// </summary>
        public string? ScreenTechnology { get; set; }

        [ExportFile]
        [ProppertyName("RAM")]
        public string? RAM { get; set; }

        [ExportFile]
        [ProppertyName("Bộ nhớ")]
        public string? Memory { get; set; }

        [ExportFile]
        [ProppertyName("Chất liệu")]
        public string? Material { get; set; }

        [ExportFile]
        [ProppertyName("Kích thước")]
        public string? Size { get; set; }

        [ExportFile]
        [ProppertyName("Trọng lượng")]
        public double? Weight { get; set; }

        [ExportFile]
        [ProppertyName("Camera")]
        public string? Camera { get; set; }

        [ExportFile]
        [ProppertyName("Pin")]
        public string? Pin { get; set; }

        [ExportFile]
        [ProppertyName("CPU")]
        public string? CPU { get; set; }

        [ExportFile]
        [ProppertyName("Hình ảnh CPU")]
        public string? ImageCPU { get; set; }

        [ExportFile]
        [ProppertyName("Hình ảnh RAM")]
        public string? ImageRAM { get; set; }

        [ExportFile]
        [ProppertyName("Hình ảnh Camera")]
        public string? ImageCamera { get; set; }

        [ExportFile]
        [ProppertyName("Hình ảnh Pin")]
        public string? ImagePin { get; set; }

        [ExportFile]
        [ProppertyName("Nội dung CPU")]
        public string? ContentCPU { get; set; }

        [ExportFile]
        [ProppertyName("Nội dung RAM")]
        public string? ContentRAM { get; set; }

        [ExportFile]
        [ProppertyName("Nội dung Camera")]
        public string? ContentCamera { get; set; }

        [ExportFile]
        [ProppertyName("Nội dung Pin")]
        public string? ContentPin { get; set; }

        [ExportFile]
        [ProppertyName("Chip")]
        public string? Chip { get; set; }

        [ExportFile]
        [ProppertyName("Nhà cung cấp")]
        public string? SupplierName { get; set; }

        [ExportFile]
        [ProppertyName("Loại sản phẩm")]
        public string? ProductTypeName { get; set; }

        public bool IsImported { get; set; } = false;

        public List<string>? ImportInvalidErrors { get; set; } = new List<string>();

    }
}
