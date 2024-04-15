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
    public class CartController : ControllerBase
    {
        ICartRepository _cartRepository;
        ICartService _cartService;

        public CartController(ICartRepository cartRepository, ICartService cartService)
        {
            _cartRepository = cartRepository;
            _cartService = cartService;
        }

        [HttpGet("{id}/{text?}")]
        public IActionResult GetByIdUser(string id, string text = "")
        {
            var result = _cartRepository.GetAllById(id, text);
            return StatusCode(200, result);
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
        public IActionResult Post(Cart entity)
        {
            var res = _cartRepository.Insert(entity);

            return StatusCode(201, res);
        }

        /// <summary>
        /// Xoá dữ liệu
        /// </summary>
        /// <param name="id">Ib bản ghi muốn xoá</param>
        /// <returns>
        /// 200: Xoá dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpDelete("delete/{id}/{accountId}")]
        public IActionResult Delete(Guid id, Guid accountId)
        {
            var entity = _cartRepository.Delete(id, accountId);
            return StatusCode(200, entity);
        }

        /// <summary>
        /// Xoá nhiều bản ghi
        /// </summary>
        /// <param name="ids">Id những bản ghi muốn xoá</param>
        /// <returns>
        /// 200: Xoá dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpDelete("delete/multipleDelete")]
        public IActionResult MultipleDelete(List<Guid> ids)
        {
            var repository = _cartRepository.MultipleDelete(ids);
            return StatusCode(200);
        }
    }
}
