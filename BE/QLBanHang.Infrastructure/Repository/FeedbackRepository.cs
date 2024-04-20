using Dapper;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.DBContext;
using QLBanHang.Core.Interfaces.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Infrastructure.Repository
{
    public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
    {
        IDbContext _dbContext;
        public FeedbackRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
