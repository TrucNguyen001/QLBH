using Microsoft.Identity.Client;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace QLBanHang.Core.Interfaces.Services
{
    public interface IInvoiceService : IBaseService<Invoice>
    {
        /// <summary>
        /// Hàm lấy ra mã loại sản phẩm lớn nhất và tự động tăng
        /// </summary>
        /// <returns>
        /// Trả về mã loại sản phẩm lớn nhất sau khi tự động tăng
        /// </returns>
        /// CreatedBy: NVTruc(1/4/2024)
        public string GetInvoiceCodeBiggest();

        public InvoiceDiscountDTOs Update(Guid id, Invoice entity);
    }
}
