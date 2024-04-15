using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;

namespace QLBanHang.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbackController : BaseController<Feedback>
    {
        public FeedbackController(IBaseRepository<Feedback> baseRepository, IBaseService<Feedback> baseService) : base(baseRepository, baseService)
        {
        }
    }
}
