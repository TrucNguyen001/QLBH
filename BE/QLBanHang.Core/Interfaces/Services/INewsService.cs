using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Services
{
    public interface INewsService : IBaseService<News>
    {
        /// <summary>
        /// Hàm lấy ra mã loại sản phẩm lớn nhất và tự động tăng
        /// </summary>
        /// <returns>
        /// Trả về mã loại sản phẩm lớn nhất sau khi tự động tăng
        /// </returns>
        /// CreatedBy: NVTruc(1/4/2024)
        public string GetNewsCodeBiggest();
    }
}
