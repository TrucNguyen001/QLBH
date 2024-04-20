using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface ICartRepository
    {
        /// <summary>
        /// Hàm lấy tất cả thông tin trong giỏ hàng
        /// </summary>
        /// <returns>Trả về danh sách sản phẩm mua</returns>
        IEnumerable<CartDTOs> GetAllById(Guid id, string text);

        /// <summary>
        /// Thêm dữ liệu
        /// </summary>
        /// <param name="cart">bản ghi thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// CreatedBy: NVTruc(9/4/2024)
        int Insert(Cart cart);

        /// <summary>
        /// Xoá dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id Entity muốn xoá</param>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// CreatedBy: NVTruc(9/4/2024)
        int Delete(Guid id, Guid accountId);

        /// <summary>
        /// Xoá nhiều bản ghi
        /// </summary>
        /// <param name="ids">Id của những bản ghi muốn xoá</param>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int MultipleDelete(List<Guid> ids);

        /// <summary>
        /// Cập nhật trang thái trong giỏ hàng
        /// </summary>
        /// <param name="invoiceId">id hoá đơn</param>
        /// <param name="accountId">id khách hàng</param>
        /// <returns>1: nếu sửa thành công</returns>
        int Update(Guid invoiceId, Guid accountId);
    }
}
