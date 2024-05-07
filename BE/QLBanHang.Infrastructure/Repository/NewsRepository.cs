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
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        IDbContext _dbContext;
        public NewsRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<News> SortDecrease()
        {
            var sqlCommand = "SELECT * FROM News";
            var entities = _dbContext.Connection.Query<News>(sql: sqlCommand);
            return entities.OrderByDescending(news => Convert.ToInt64(news.NewsCode.Substring(3)));
        }
    }
}
