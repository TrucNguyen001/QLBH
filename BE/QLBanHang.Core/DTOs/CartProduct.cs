using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    // Thống kê loại sản phẩm bán chạy nhấy
    public class CartProduct
    {
        public string ProductTypeName { get; set; }

        public int Total {  get; set; }
    }
}
