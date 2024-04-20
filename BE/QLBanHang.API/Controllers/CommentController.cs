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
    public class CommentController : ControllerBase
    {
        ICommentRepository _commentRepository;
        ICommentService _commentService;

        public CommentController(ICommentRepository commentRepository, ICommentService commentService)
        {
            _commentRepository = commentRepository;
            _commentService = commentService;
        }

        [HttpGet("comment-feedback/{id}")]
        public IActionResult GetCommentById(string id)
        {
            var entities = _commentRepository.GetCommentById(id);
            return StatusCode(200, entities);
        }

        [HttpGet("code-biggest")]
        public IActionResult GetProductTypeCode()
        {
            var code = _commentService.GetCommentCodeBiggest();
            return StatusCode(200, code);
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
        public IActionResult Post(Comment entity)
        {
            var record = _commentService.Insert(entity);

            var res2 = _commentRepository.Insert(record);

            return StatusCode(201, res2);
        }

        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>
        /// 200: Nếu có dữ liệu
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpGet("{status}")]
        public IActionResult GetAll(int status)
        {
            var entities = _commentRepository.GetAll(status);
            return StatusCode(200, entities);
        }

        /// <summary>
        /// Phân trang những bản ghi
        /// </summary>
        /// <param name="pageSize">Số lượng trang</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="text">Từ tìm kiếm trong trang</param>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        ///</returns>
        /// CreatedBy: NVTruc(24/12/2023)
        [HttpGet("paging/{status}")]
        public IActionResult Paging(int pageSize, int pageIndex, string text = "", int status = 1)
        {
            var validatePage = _commentService.GetPaging(pageSize, pageIndex, text);
            var entities = _commentRepository.GetPaging(pageSize, pageIndex, text, status);

            // Lấy ra tổng số bản ghi theo phân trang
            var total = _commentRepository.GetByText(text, status).Count();

            var result = new
            {
                ToTalRecord = total,
                ListRecord = entities,
            };

            return StatusCode(200, result);
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>
        /// 200: Nếu có dữ liệu
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpGet("getById/{id}")]
        public IActionResult GetById(Guid id)
        {
            var entity = _commentRepository.GetById(id);
            return StatusCode(200, entity);
        }
    }
}
