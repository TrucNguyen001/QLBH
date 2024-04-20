using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using QLBanHang.Core.Service;
using QLBanHang.Infrastructure.Repository;

namespace QLBanHang.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductRepository _productRespository;
        IProductService _productService;

        public ProductController(IProductRepository productRepository, IProductService productService)
        {
            _productRespository = productRepository;
            _productService = productService;
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
        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _productRespository.GetAll();
            return StatusCode(200, entities);
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
            var entity = _productRespository.GetById(id);
            return StatusCode(200, entity);
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
        public IActionResult Post(ProductDTOs entity)
        {
            var res = _productService.Insert(entity);

            var res2 = _productRespository.Insert(entity);

            return StatusCode(201, res);
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
        public IActionResult Put(ProductDTOs entity, Guid id)
        {
            var service = _productService.Update(entity, id);

            var respository = _productRespository.Update(entity, id);

            return StatusCode(200, respository);
        }

        /// <summary>
        /// Xoá dữ liệu
        /// </summary>
        /// <param name="misaEntityId">Ib bản ghi muốn xoá</param>
        /// <returns>
        /// 200: Xoá dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var entity = _productRespository.Delete(id);
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
        [HttpDelete("delete/{status}/multipleDelete/")]
        public IActionResult MultipleDelete(List<Guid> ids, int status)
        {
            var repository = _productRespository.MultipleDelete(ids, status);
            return StatusCode(200, repository);
        }

        /// <summary>
        /// Thêm nhiều bản ghi
        /// </summary>
        /// <param name="records">Danh sách bản ghi muốn thêm</param>
        /// <returns>
        /// 201: Xoá dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        /// CreatedBy: NVTruc(31/3/2024)
        [HttpPost("multiplePost")]
        public IActionResult MultiplePost(List<Product> records)
        {
            var repository = _productRespository.MultiplePost(records);
            return StatusCode(201, repository);
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
            var validatePage = _productService.GetPaging(pageSize, pageIndex, text);
            var entities = _productRespository.GetPaging(pageSize, pageIndex, text, status);

            // Lấy ra tổng số bản ghi theo phân trang
            var total = _productRespository.GetByText(text, status).Count();

            var result = new
            {
                ToTalRecord = total,
                ListRecord = entities,
            };

            return StatusCode(200, result);
        }

        /// <summary>
        /// Phân trang những bản ghi
        /// </summary>
        /// <param name="optionSelect">Option lựa chọn</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="text">Từ tìm kiếm trong trang</param>
        /// <param name="productTypeId">Loại sản phẩm</param>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        ///</returns>
        /// CreatedBy: NVTruc(24/12/2023)
        [HttpGet("paging-user")]
        public IActionResult PagingUser(int pageIndex, string text = "", int optionSelect = 0, string productTypeId = "")
        {

            var entities = _productRespository.GetPagingUser(pageIndex, text, optionSelect, productTypeId);
            var total = _productRespository.TotalRecordPaging(text, productTypeId).Count();

            var result = new
            {
                ToTalRecord = total,
                ListRecord = entities,
            };

            return StatusCode(200, result);
        }

        /// <summary>
        /// Export tất cả bản ghi sang file excel
        /// </summary>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        ///  CreatedBy: NVTruc(24/12/2023)
        [HttpGet("ExportFile")]
        public IActionResult ExportExcel()
        {
            var result = _productService.ExportExcel<Product>();
            return File(result.FileStream, result.FileContent, result.FileName);
        }

        /// <summary>
        /// Lấy ra mã sản phẩm lớn nhất và tự động tăng 1
        /// </summary>
        /// <returns>
        /// 200: Xuất dữ liệu thành công
        /// 400: Lỗi nghiệp vụ
        /// 500: Nếu có exception
        /// </returns>
        ///  CreatedBy: NVTruc(2/4/2024)
        [HttpGet("code-biggest")]
        public IActionResult GetproductCode()
        {
            var code = _productService.GetProductCodeBiggest();
            return StatusCode(200, code);
        }
    }
}
