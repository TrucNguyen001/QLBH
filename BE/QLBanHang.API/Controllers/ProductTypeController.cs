using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using System.Security.Cryptography.X509Certificates;

namespace QLBanHang.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductTypeController : BaseController<ProductType>
    {
        IProductTypeRepository _productTypeRepository;
        IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeRepository productTypeRepository, IProductTypeService productTypeService) : base(productTypeRepository, productTypeService)
        {
            _productTypeRepository = productTypeRepository;
            _productTypeService = productTypeService;
        }

        /// <summary>
        /// Lấy ra mã lớn nhất và tự động tăng 1
        /// </summary>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        ///  CreatedBy: NVTruc(1/4/2024)
        [HttpGet("code-biggest")]
        public IActionResult GetProductTypeCode()
        {
            var code = _productTypeService.GetProductTypeCodeBiggest();
            return StatusCode(200, code);
        }
    }
}
