using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using QLBanHang.Core.Service;

namespace QLBanHang.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NewsController : BaseController<News>
    {
        INewsRepository _newRepository;
        INewsService _newsService;
        public NewsController(INewsRepository newsRepository, INewsService newsService) : base(newsRepository, newsService)
        {
            _newsService = newsService;
            _newRepository = newsRepository;
        }

        /// <summary>
        /// Lấy ra mã nhân viên lớn nhất và tự động tăng 1
        /// </summary>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        ///  CreatedBy: NVTruc(2/4/2024)
        [HttpGet("code-biggest")]
        public IActionResult GetSupplierCode()
        {
            var code = _newsService.GetNewsCodeBiggest();
            return StatusCode(200, code);
        }
    }
}
