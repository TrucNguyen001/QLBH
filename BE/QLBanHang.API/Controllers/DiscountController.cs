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
    public class DiscountController : BaseController<Discount>
    {
        IDiscountService _discountService;
        IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository, IDiscountService discountService) : base(discountRepository, discountService)
        {
            _discountService = discountService;
            _discountRepository = discountRepository;
        }
        /// <summary>
        /// Lấy ra mã
        /// </summary>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        ///  CreatedBy: NVTruc(6/4/2024)
        [HttpGet("code-biggest")]
        public IActionResult GetProductTypeCode()
        {
            var code = _discountService.GetDiscountCode();
            return StatusCode(200, code);
        }

        /// <summary>
        /// Lấy ra mã
        /// </summary>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        ///  CreatedBy: NVTruc(6/4/2024)
        [HttpGet("discountCode/{discountCode}")]
        public IActionResult GetByDiscountCode(string discountCode)
        {
            var record = _discountRepository.GetByDiscountCode(discountCode);
            return StatusCode(200, record);
        }
    }
}
