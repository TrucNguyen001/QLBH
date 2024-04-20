using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;

namespace QLBanHang.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : class
    {
        IBaseRepository<T> _baseRepository;
        IBaseService<T> _baseService;

        public BaseController(IBaseRepository<T> baseRepository, IBaseService<T> baseService)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
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
        [HttpGet("getAll/{status}")]
        public IActionResult GetAll(int status)
        {
            var entities = _baseRepository.GetAll(status);
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
            var entity = _baseRepository.GetById(id);
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
        public IActionResult Post(T entity)
        {
            var res = _baseService.Insert(entity);

            var res2 = _baseRepository.Insert(entity);

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
        public IActionResult Put(T entity, Guid id)
        {
            var service = _baseService.Update(entity, id);

            var respository = _baseRepository.Update(entity, id);

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
        [HttpDelete("delete/{status}/{id}")]
        public IActionResult Delete(Guid id, int status)
        {
            var entity = _baseRepository.Delete(id, status);
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
            var repository = _baseRepository.MultipleDelete(ids, status);
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
        public IActionResult MultiplePost(List<T> records)
        {
            var repository = _baseRepository.MultiplePost(records);
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
            var validatePage = _baseService.GetPaging(pageSize, pageIndex, text);
            var entities = _baseRepository.GetPaging(pageSize, pageIndex, text, status);

            // Lấy ra tổng số bản ghi theo phân trang
            var total = _baseRepository.GetByText(text, status).Count();

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
            var result = _baseService.ExportExcel<T>();
            return File(result.FileStream, result.FileContent, result.FileName);
        }
    }
}
