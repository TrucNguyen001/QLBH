using Dapper;
using Microsoft.Identity.Client;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.DBContext;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.MISAEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Infrastructure.Repository
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        IDbContext _dbContext;
        public InvoiceRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Invoice GetById(Guid id)
        {
            var sqlCommand = $"SELECT * FROM Invoice WHERE InvoiceId = @entityId";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            var entity = _dbContext.Connection.QueryFirstOrDefault<Invoice>(sql: sqlCommand, param: paramet);

            return entity;
        }

        public IEnumerable<CartDTOs> GetByUserId(Guid id, int status)
        {
            var sqlCommand = "Proc_GetInvoiceByUserId";
            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("id", id);
            paramet.Add("status", status);

            var entities = _dbContext.Connection.Query<CartDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return entities;
        }

        public IEnumerable<CartDTOs> GetInvoiceById(Guid invoiceId)
        {
            var sqlCommand = "Proc_GetInvoiceById";
            DynamicParameters paramet = new DynamicParameters();

            //paramet.Add("accountId", accountId);
            paramet.Add("invoiceId", invoiceId);

            var entities = _dbContext.Connection.Query<CartDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return entities;
        }

        public InvoiceDiscountDTOs getInvoiceDiscountId(Guid invoiceId)
        {
            var sqlCommand = "Proc_GetInvoiceDiscount";
            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("invoiceId", invoiceId);

            var entity = _dbContext.Connection.QueryFirstOrDefault<InvoiceDiscountDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return entity;
        }

        public IEnumerable<CartDTOs> GetInvoiceSuccessByUserId(Guid id)
        {
            var sqlCommand = "Proc_GetInvoiceSuccessByUserId";
            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("id", id);

            var entities = _dbContext.Connection.Query<CartDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return entities;
        }

        public IEnumerable<Invoice> SortDecrease()
        {
            var sqlCommand = "SELECT * FROM Invoice";
            var entities = _dbContext.Connection.Query<Invoice>(sql: sqlCommand);
            return entities.OrderByDescending(invoice => Convert.ToInt64(invoice.InvoiceCode.Substring(3)));
        }

        public int Update(Invoice entity, Guid id, Guid userId)
        {
            string stringUpdate = "";

            var props = typeof(Invoice).GetProperties();

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            for (int i = 1; i < props.Length; i++)
            {
                stringUpdate += $"{props[i].Name} = @{props[i].Name},";

                paramet.Add($"@{props[i].Name}", props[i].GetValue(entity));
            }


            stringUpdate = stringUpdate.Substring(0, stringUpdate.Length - 1);

            var sqlCommand = $"UPDATE Invoice SET {stringUpdate} WHERE InvoiceId = @entityId";

            var update = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);
            return 1;
        }

        public IEnumerable<Invoice> GetPaging(int pageSize, int pageIndex, string text, int status)
        {
            var sqlCommand = $"SELECT * FROM Invoice WHERE (InvoiceCode LIKE @text OR UserName LIKE @text) AND StatusInvoice = @status ORDER BY COALESCE(CreatedDate) DESC";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);
            paramet.Add("@status", status);

            var entities = _dbContext.Connection.Query<Invoice>(sql: sqlCommand, param: paramet);
            entities = entities.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return entities;
        }

        public IEnumerable<Invoice> GetByText(string text, int status)
        {
            var sqlCommand = $"SELECT * FROM Invoice WHERE (InvoiceCode LIKE @text OR UserName LIKE @text) AND StatusInvoice = @status";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);
            paramet.Add("@status", status);

            var entities = _dbContext.Connection.Query<Invoice>(sql: sqlCommand, param: paramet);

            return entities;
        }

        public IEnumerable<Invoice> GetAll(int year)
        {
            var sqlCommand = "";
            sqlCommand = $"SELECT * FROM Invoice Where StatusInvoice = 4";
            if (year > 0)
            {
                sqlCommand = $"SELECT * FROM Invoice Where StatusInvoice = 4 AND YEAR(CreatedDate) = @year";
            }
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@year", year);
            var entities = _dbContext.Connection.Query<Invoice>(sql: sqlCommand, param: paramet);
            return entities;
        }
    }
}
