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
    public class ProductTypeRepository : BaseRepository<ProductType>, IProductTypeRepository
    {
        IDbContext _dbContext;
        public ProductTypeRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ProductType> SortDecrease()
        {
            var sqlCommand = $"SELECT * FROM ProductType";
            var entities = _dbContext.Connection.Query<ProductType>(sql: sqlCommand);
            return entities.OrderByDescending(productType => Convert.ToInt64(productType.ProductTypeCode.Substring(3)));
        }
    }
}
