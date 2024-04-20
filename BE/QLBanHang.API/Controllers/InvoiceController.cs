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
    public class InvoiceController : ControllerBase
    {
        IInvoiceRepository _invoiceRepository;
        IInvoiceService _invoiceService;
        ICartRepository _cartRepository;
        IDiscountService _discountService;

        public InvoiceController(IInvoiceRepository invoiceRepository, IInvoiceService invoiceService, ICartRepository cartRepository, IDiscountService discountService)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceService = invoiceService;
            _cartRepository = cartRepository;
            _discountService = discountService;
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
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var entity = _invoiceRepository.GetById(id);
            return StatusCode(200, entity);
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        /// <param name="entity">Bản ghi</param>
        /// <param name="id">Ib bản ghi muốn sửa</param>
        /// <returns>
        /// 200: Sửa dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpPut("put/{id}/{accountId}")]
        public IActionResult Put(Guid id, Guid accountId, Invoice entity)
        {
            var service = _invoiceService.Update(id, entity);

            var respository = _invoiceRepository.Update(entity, id, accountId);

            _cartRepository.Update(id, accountId);
            if(service != null)
            {
                _discountService.UpdateInputNumber((Guid)service.DiscountId, service.InputNumber - 1);
            }

            return StatusCode(200, respository);
        }

        // <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>
        /// 200: Nếu có dữ liệu
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpGet("user/{id}/{status}")]
        public IActionResult GetByUserId(Guid id, int status)
        {
            var entity = _invoiceRepository.GetByUserId(id, status);
            return StatusCode(200, entity);
        }

        // <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>
        /// 200: Nếu có dữ liệu
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpGet("user-success/{id}")]
        public IActionResult GetInvoiceSuccessByUserId(Guid id)
        {
            var entity = _invoiceRepository.GetInvoiceSuccessByUserId(id);
            return StatusCode(200, entity);
        }

        /// <summary>
        /// Lấy bản ghi liên kết theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>
        /// 200: Nếu có dữ liệu
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpGet("invoice/{invoiceId}")]
        public IActionResult GetInvoiceById(Guid invoiceId)
        {
            var entity = _invoiceRepository.GetInvoiceById(invoiceId);
            return StatusCode(200, entity);
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
            var validatePage = _invoiceService.GetPaging(pageSize, pageIndex, text);
            var entities = _invoiceRepository.GetPaging(pageSize, pageIndex, text, status);

            // Lấy ra tổng số bản ghi theo phân trang
            var total = _invoiceRepository.GetByText(text, status).Count();

            var result = new
            {
                ToTalRecord = total,
                ListRecord = entities,
            };

            return StatusCode(200, result);
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        /// <param name="entity">Bản ghi</param>
        /// <param name="id">Ib bản ghi muốn sửa</param>
        /// <returns>
        /// 200: Sửa dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpPut("put/{id}")]
        public IActionResult UpdateById(Guid id, Invoice entity)
        {
            var repository = _invoiceRepository.Update(entity, id);

            return StatusCode(200, repository);
        }

        [HttpDelete("put/multipleUpdate/{status}")]
        public IActionResult MultipleUpdate(List<Guid> ids, int status)
        {
            var repository = _invoiceRepository.MultipleUpdateStatus(ids, status);
            return StatusCode(200, repository);
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
        [HttpGet("getAll/{year}")]
        public IActionResult GetAll(int year)
        {
            var entities = _invoiceRepository.GetAll(year);
            return StatusCode(200, entities);
        }

    }
}
