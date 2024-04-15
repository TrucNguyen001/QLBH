using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface IAccountRepository:IBaseRepository<Account>
    {
        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="id">Id Entity muốn lấy</param>
        /// <returns>Trả về bản nghi tìm</returns>
        /// CreatedBy: NVTruc(31/3/2024)
        IEnumerable<Account> GetByRole(string role);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageIndex">Vị trí bản ghi</param>
        /// <param name="text">Tìm kiểm theo Mã, Họ tên, Số điện thoại</param>
        /// <returns>Trả về số lượng bản ghi</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<Account> GetPaging(int pageSize, int pageIndex, string text, string role);

        /// <summary>
        /// Lấy bản ghi theo text
        /// </summary>
        /// <param name="text">nội dung tìm kiếm</param>
        /// <returns>Danh sách tìm kiêm</returns>
        /// CreatedBy: NVTruc(1/4/2024)
        IEnumerable<Account> GetByTextUser(string text, string role);

        /// <summary>
        /// Đăng nhâp
        /// </summary>
        /// <param name="account">Tài khoản dùng để đăng nhập</param>
        /// <returns>Trả về 1 nếu đăng nhập thành công</returns>
        Account Login(AccountLogin account, string role);

        /// <summary>
        /// Tìm kiếm tài khoản theo tên tài khoản
        /// </summary>
        /// <param name="username">Tên tài khoản</param>
        /// <returns>Trả về thông tin tài khoản</returns>
        /// Created by: Nguyễn Văn Trúc(17/2/2024)
        Account GetByUserNameRole(string username, string role);

        /// <summary>
        /// Dùng trong refresh token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Account GetByUserName(string username);

        /// <summary>
        /// Tìm kiếm tài khoản theo số điện thoại
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <returns>Trả về thông tin tài khoản</returns>
        /// Created by: Nguyễn Văn Trúc(17/2/2024)
        Account GetByPhoneNumber(string phoneNumber);

        /// <summary>
        /// Tìm kiếm tài khoản theo email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Trả về thông tin tài khoản</returns>
        /// Created by: Nguyễn Văn Trúc(17/2/2024)
        Account GetByEmail(string email);

        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="accountNew">Tài khoản muốn cập nhật</param>
        /// <returns>trả về 1 nếu cập nhật thành công</returns>
        /// Created by: Nguyễn Văn Trúc(19/2/2024)
        int UpdateAccount(Account accountNew);

        /// <summary>
        /// Thêm thông tin tài khoản
        /// </summary>
        /// <param name="account">tài khoản muốn thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// Created by: Nguyễn Văn Trúc(19/2/2024)
        int InsertAccount(Account account);

        /// <summary>
        /// Update lại mật khẩu
        /// </summary>
        /// <param name="accountId">Id tài khoản</param>
        /// <param name="account">Tài khoản</param>
        /// <returns>Trả về 1 nếu update thành công</returns>
        /// Created by: Nguyễn Văn Trúc(9/3/2024)
        int UpdatePassword(Guid accountId, Account account);

        /// <summary>
        /// Hàm giúp sắp xếp danh sách giảm dần theo mã
        /// </summary>
        /// <returns>
        /// Trả về danh sách giảm dần theo mã
        /// </returns>
        /// CreateBy: NVTruc(2/4/2024)
        public IEnumerable<Account> SortDecrease();
    }
}
