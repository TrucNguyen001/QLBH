using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface IProductRepository
    {
        /// <summary>
        /// Hàm giúp sắp xếp danh sách giảm dần theo mã
        /// </summary>
        /// <returns>
        /// Trả về danh sách giảm dần theo mã
        /// </returns>
        /// CreateBy: NVTruc(2/4/2024)
        public IEnumerable<Product> SortDecrease();

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>Trả về toàn bộ bản ghi</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        IEnumerable<Product> GetAll();

        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id Entity muốn lấy</param>
        /// <returns>Trả về bản nghi tìm</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        ProductDTOs GetById(Guid id);

        /// <summary>
        /// Thêm dữ liệu
        /// </summary>
        /// <param name="entity">bản ghi thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Insert(ProductDTOs entity);



        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">bản ghi sửa</param>
        /// <param name="id">Id Entity muốn sửa</param>
        /// <returns>Trả về 1 nếu sửa thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Update(ProductDTOs entity, Guid id);

        /// <summary>
        /// Xoá dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id Entity muốn xoá</param>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Delete(Guid id);

        /// <summary>
        /// Xoá nhiều bản ghi
        /// </summary>
        /// <param name="ids">Id của những bản ghi muốn xoá</param>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int MultipleDelete(List<Guid> ids);

        /// <summary>
        /// Thêm nhiều bản ghi
        /// </summary>
        /// <param name="entitys">Danh sách bản ghi muốn thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int MultiplePost(List<Product> entitys);

        /// <summary>
        /// Kiểm tra trùng mã 
        /// </summary>
        /// <param name="code">Mã kiểm tra</param>
        /// <returns>
        /// true: Mã đã tồn tại
        /// false: Mã chưa tồn tại
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        bool CheckDuplicateCode(string code);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageIndex">Vị trí bản ghi</param>
        /// <param name="text">Tìm kiểm theo Mã, Họ tên, Số điện thoại</param>
        /// <returns>Trả về số lượng bản ghi</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<ProductDTOs> GetPaging(int pageSize, int pageIndex, string text);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="optionSelect">Option lựa chọn</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="text">Từ tìm kiếm trong trang</param>
        /// <param name="productTypeId">Loại sản phẩm</param>
        /// <returns>Trả về số lượng bản ghi</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<Product> GetPagingUser(int pageIndex, string text, int optionSelect, string productTypeId);

        IEnumerable<Product> TotalRecordPaging(string text, string productTypeId);

        /// <summary>
        /// Lấy bản ghi theo text
        /// </summary>
        /// <param name="text">nội dung tìm kiếm</param>
        /// <returns>Danh sách tìm kiêm</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<Product> GetByText(string text);
    }
}
