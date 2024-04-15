using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;

namespace QLBanHang.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NewsController : BaseController<News>
    {
        public NewsController(IBaseRepository<News> baseRepository, IBaseService<News> baseService) : base(baseRepository, baseService)
        {
        }
    }
}
