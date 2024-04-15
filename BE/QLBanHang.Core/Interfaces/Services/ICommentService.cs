using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Services
{
    public interface ICommentService : IBaseService<Comment>
    {
        /// <summary>
        /// Hàm lấy ra mã comment lớn nhất và tự động tăng
        /// </summary>
        /// <returns>
        /// Trả về mã comment lớn nhất sau khi tự động tăng
        /// </returns>
        /// CreatedBy: NVTruc(1/4/2024)
        public string GetCommentCodeBiggest();

        /// <summary>
        /// Thêm comment
        /// </summary>
        /// <param name="comment">Nội dung bình luận</param>
        /// <returns>thành công trả về bản ghi</returns>
        public Comment Insert(Comment comment);
    }
}
