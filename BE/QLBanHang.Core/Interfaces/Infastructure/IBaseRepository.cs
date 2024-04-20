using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>Trả về toàn bộ bản ghi</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        IEnumerable<T> GetAll(int status);

        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id Entity muốn lấy</param>
        /// <returns>Trả về bản nghi tìm</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        T GetById(Guid id);

        /// <summary>
        /// Thêm dữ liệu
        /// </summary>
        /// <param name="entity">bản ghi thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Insert(T entity);



        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">bản ghi sửa</param>
        /// <param name="id">Id Entity muốn sửa</param>
        /// <returns>Trả về 1 nếu sửa thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Update(T entity, Guid id);

        /// <summary>
        /// Xoá dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id Entity muốn xoá</param>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int Delete(Guid id, int status);

        /// <summary>
        /// Xoá nhiều bản ghi
        /// </summary>
        /// <param name="ids">Id của những bản ghi muốn xoá</param>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int MultipleDelete(List<Guid> ids, int status);

        /// <summary>
        /// Thêm nhiều bản ghi
        /// </summary>
        /// <param name="entitys">Danh sách bản ghi muốn thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int MultiplePost(List<T> entitys);

        /// <summary>
        /// Sửa nhiều bản ghi
        /// </summary>
        /// <param name="ids">Id của những bản ghi muốn xoá</param>
        /// <returns>Trả về 1 nếu xoá thành công</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        int MultipleUpdateStatus(List<Guid> ids, int status);

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
        IEnumerable<T> GetPaging(int pageSize, int pageIndex, string text, int status);

        /// <summary>
        /// Lấy bản ghi theo text
        /// </summary>
        /// <param name="text">nội dung tìm kiếm</param>
        /// <returns>Danh sách tìm kiêm</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<T> GetByText(string text, int status);
    }
}
