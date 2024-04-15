using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }

        public string AccountCode { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public int? Gender { get; set; }

        public string? Role { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Số lần truy cập thất bại
        /// </summary>
        public int? AccessFailedCount { get; set; }

        /// <summary>
        /// Chuỗi ngẫu nhiên được tạo ra mỗi khi dữ liệu người dùng được cập nhật
        /// </summary>
        public string? ConcurrencyStamp { get; set; }

        /// <summary>
        /// Số điện thoại được xác nhận chưa
        /// false: chưa
        /// true: Rồi
        /// </summary>
        public bool? PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Email được xác nhận chưa
        /// false: chưa
        /// true: Rồi
        /// </summary>
        public bool? EmailConfirmed { get; set; }

        /// <summary>
        /// Email theo định dạng
        /// </summary>
        public string? NormalizedEmail { get; set; }

        /// <summary>
        /// Tên tài khoản theo định dạng
        /// </summary>
        public string? NormalizedUserName { get; set; }

        /// <summary>
        /// Tài khoản bị khoá không
        /// False: Không
        /// True: Có
        /// </summary>
        public bool? LockoutEnabled { get; set; }

        /// <summary>
        /// Chuỗi ngẫu nhiên tăng độ bảo mật
        /// </summary>
        public string? SecurityStamp { get; set; }

        /// <summary>
        /// Xác nhận 2 lớp
        /// True: Đã có
        /// False: Chưa
        /// </summary>
        public bool? TwoFactorEnabled { get; set; }

        /// <summary>
        /// Sau khoảng thời gian sẽ khoá tài khoản
        /// </summary>
        public int? LockoutEnd { get; set; }

        /// <summary>
        /// Làm mới lại AccessToken
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Thời gian tồn tại RefreshToken
        /// </summary>
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
