using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Service
{
    public class CommentService : BaseService<Comment>, ICommentService
    {
        ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository) : base(commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public string GetCommentCodeBiggest()
        {
            var comments = _commentRepository.SortDecrease();
            if(string.IsNullOrEmpty(comments.ToString())) {
                return "BL-00001";
            }
            else
            {
                Comment comment = comments.FirstOrDefault();
                var commentCode = comment.CommentCode;
                var numberCode = commentCode.Substring(3);
                var numberCodeBiggest = Convert.ToInt64(numberCode) + 1;
                numberCode = PadLeftCustom(Convert.ToString(numberCodeBiggest), numberCode.Length, '0');
                commentCode = commentCode.Substring(0, 3) + numberCode;
                return commentCode;
            }
        }

        public Comment Insert(Comment comment)
        {
            comment.CommentCode = GetCommentCodeBiggest();
            comment.CreatedDate = DateTime.Now;
            comment.Status = 1;
            return comment;
        }

        /// <summary>
        /// Hàm kiểm tra chuỗi
        /// </summary>
        /// <param name="str">chuỗi truyền vào</param>
        /// <param name="totalWidth">Độ dài ban đầu</param>
        /// <param name="paddingChar">Kí tự thêm vào</param>
        /// <returns>
        /// Trả về chuỗi ban đầu nếu có độ dài >= độ dài ban đầu
        /// Trả về chuỗi thêm kí tự đằng trước nếu độ dài < độ dài ban đầu
        /// </returns>
        /// CreatedBy: NVTruc(1/4/2024)
        public string PadLeftCustom(string str, int totalWidth, char paddingChar)
        {
            if (str.Length >= totalWidth)
            {
                return str;
            }
            else
            {
                return new string(paddingChar, totalWidth - str.Length) + str;
            }
        }
    }
}
