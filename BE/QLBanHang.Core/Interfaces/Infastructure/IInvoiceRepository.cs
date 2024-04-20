using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        /// <summary>
        /// Hàm giúp sắp xếp danh sách giảm dần theo mã
        /// </summary>
        /// <returns>
        /// Trả về danh sách giảm dần theo mã
        /// </returns>
        /// CreateBy: NVTruc(2/4/2024)
        public IEnumerable<Invoice> SortDecrease();

        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id Entity muốn lấy</param>
        /// <returns>Trả về bản nghi tìm</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        Invoice GetById(Guid id);

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">bản ghi sửa</param>
        /// <param name="id">Id Entity muốn sửa</param>
        /// <param name="userId">id khách hàng</param>
        /// <returns>Trả về 1 nếu sửa thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Update(Invoice entity, Guid id, Guid userId);

        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id User muốn lấy</param>
        /// <returns>Trả về bản nghi tìm</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        IEnumerable<CartDTOs> GetByUserId(Guid id, int status);

        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id User muốn lấy</param>
        /// <returns>Trả về bản nghi tìm</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        IEnumerable<CartDTOs> GetInvoiceSuccessByUserId(Guid id);

        /// <summary>
        /// Lấy bản ghi liên kết theo id
        /// </summary>
        /// <param name="accountId">id tài khoản</param>
        /// <param name="invoiceId">id hoá đơn</param>
        /// <returns>trả về bản ghi</returns>
        IEnumerable<CartDTOs> GetInvoiceById(Guid invoiceId);

        InvoiceDiscountDTOs getInvoiceDiscountId(Guid invoiceId);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageIndex">Vị trí bản ghi</param>
        /// <param name="text">Tìm kiểm theo Mã, Họ tên, Số điện thoại</param>
        /// <returns>Trả về số lượng bản ghi</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<Invoice> GetPaging(int pageSize, int pageIndex, string text, int status);

        /// <summary>
        /// Lấy bản ghi theo text
        /// </summary>
        /// <param name="text">nội dung tìm kiếm</param>
        /// <returns>Danh sách tìm kiêm</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<Invoice> GetByText(string text, int status);

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>Trả về toàn bộ bản ghi</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        IEnumerable<Invoice> GetAll(int year);


    }
}
