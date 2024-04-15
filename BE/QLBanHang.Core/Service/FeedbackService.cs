using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Service
{
    public class FeedbackService : BaseService<Feedback>, IFeedbackService
    {
        public FeedbackService(IBaseRepository<Feedback> baseRepository) : base(baseRepository)
        {
        }
    }
}
