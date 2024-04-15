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
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        IDbContext _dbContext;
        public DiscountRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Discount GetByDiscountCode(string discountCode)
        {
            var sqlCommand = $"SELECT * FROM Discount WHERE DiscountCode = @discountCode";

            DynamicParameters paramt = new DynamicParameters();
            paramt.Add("@discountCode", discountCode);

            var result = _dbContext.Connection.QueryFirstOrDefault<Discount>(sql: sqlCommand, param: paramt);

            return result;
        }

        public int UpdateInputNumber(Guid discountId, int number)
        {
            var sqlCommand = "UPDATE Discount SET InputNumber = @inputNumber WHERE DiscountId = @discountId";

            DynamicParameters paramt = new DynamicParameters();

            paramt.Add("@inputNumber", number);
            paramt.Add("@discountId", discountId);

            _dbContext.Connection.Execute(sql: sqlCommand, param: paramt);

            return 1;
        }
    }
}
