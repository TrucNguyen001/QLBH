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
    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        IDbContext _dbContext;
        public SupplierRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Supplier> SortDecrease()
        {
            var sqlCommand = "SELECT * FROM Supplier";
            var entities = _dbContext.Connection.Query<Supplier>(sql: sqlCommand);
            return entities.OrderByDescending(supplier => Convert.ToInt64(supplier.SupplierCode.Substring(3)));
        }
    }
}
