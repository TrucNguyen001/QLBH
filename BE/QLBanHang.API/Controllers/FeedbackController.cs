using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using QLBanHang.Core.Service;
using QLBanHang.Infrastructure.Repository;

namespace QLBanHang.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        IFeedbackRepository _feedbackRepository;
        IFeedbackService _feedbackService;
        ICommentRepository _commentRepository;
        public FeedbackController(IFeedbackRepository feedbackRepository, IFeedbackService feedbackService, ICommentRepository commentRepository)
        {
            _feedbackRepository = feedbackRepository;
            _feedbackService = feedbackService;
            _commentRepository = commentRepository;
        }

        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="entity">bản ghi thêm</param>
        /// <returns>
        /// 201: Thêm dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpPost("post")]
        public IActionResult Post(Feedback entity)
        {
            var res = _feedbackService.Insert(entity);

            var res2 = _feedbackRepository.Insert(entity);

            if(entity.Status == 1)
            {
                var comment = _commentRepository.GetByIdComment(entity.CommentId);
                comment.Status = 0;
                _commentRepository.Update(comment, comment.CommentId);
            }

            return StatusCode(201, res);
        }
    }
}
