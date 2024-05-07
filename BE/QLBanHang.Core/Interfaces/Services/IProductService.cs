using MISA.AMISDemo.Core.Entities;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Hàm lấy ra mã sản phẩm lớn nhất và tự động tăng
        /// </summary>
        /// <returns>
        /// Trả về mã sản phẩm lớn nhất sau khi tự động tăng
        /// </returns>
        /// CreatedBy: NVTruc(2/4/2024)
        public string GetProductCodeBiggest();

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">bản ghi thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Insert(ProductDTOs entity);

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        /// <param name="entity">bản ghi sửa</param>
        /// <param name="id">Entity Id</param>
        /// <returns>Trả về 1 nếu sửa thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Update(ProductDTOs entity, Guid id);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageIndex">Vị trí bản ghi</param>
        /// <param name="text">Tìm kiếm theo Mã, Họ tên, Số điện thoại</param>
        /// <returns>Trả 1 nếu lấy dữ liệu thành công</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        int GetPaging(int pageSize, int pageIndex, string text);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="optionSelect">Option lựa chọn</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="text">Từ tìm kiếm trong trang</param>
        /// <param name="productTypeId">Loại sản phẩm</param>
        /// <returns>Trả về 1 nếu kiểm tra thành công</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        int GetPagingUser(int pageIndex, string text, int optionSelect, string productTypeId);

        /// <summary>
        /// Thực hiện xuất thành file excel
        /// </summary>
        /// <returns>
        /// Trả về danh sách nhân viên file excel
        /// </returns>
        /// CreatedBy: NVTruc (26/1/2024)
        FileExport ExportExcel();
    }
}
