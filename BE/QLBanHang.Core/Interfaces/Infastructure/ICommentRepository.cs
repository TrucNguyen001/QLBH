using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        IEnumerable<FeedbackDTOs> GetCommentById(string id);

        /// <summary>
        /// Hàm giúp sắp xếp danh sách giảm dần theo mã
        /// </summary>
        /// <returns>
        /// Trả về danh sách giảm dần theo mã
        /// </returns>
        /// CreateBy: NVTruc(2/4/2024)
        public IEnumerable<Comment> SortDecrease();

        /// <summary>
        /// Thêm comment
        /// </summary>
        /// <param name="comment">Nội dung bình luận</param>
        /// <returns>1 nếu thành công</returns>
        public int Insert(Comment comment);

        IEnumerable<FeedbackDTOs> GetById(Guid id);

        Comment GetByIdComment(Guid id);
    }
}
