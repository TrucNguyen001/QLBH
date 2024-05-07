using Dapper;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.DBContext;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Infrastructure.Repository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        IDbContext _dbContext;
        public AccountRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Account> SortDecrease()
        {
            var sqlCommand = $"SELECT * FROM Account WHERE Role = @role";

            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@role", "User");

            var entities = _dbContext.Connection.Query<Account>(sql: sqlCommand, param: paramet);
            return entities.OrderByDescending(account => Convert.ToInt64(account.AccountCode.Substring(3)));
        }

        /// <summary>
        /// Tìm kiếm tài khoản theo tên tài khoản
        /// </summary>
        /// <param name="username">Tên tài khoản</param>
        /// <returns>Trả về thông tin tài khoản</returns>
        /// Created by: Nguyễn Văn Trúc(17/2/2024)
        public Account GetByUserNameRole(string username, string role)
        {
            var sqlCommand = "";
            if(role == "User")
            {
                sqlCommand = "SELECT * FROM Account WHERE UserName = @username AND Role = 'User' AND Status = 1";
            }
            else
            {
                sqlCommand = "SELECT * FROM Account WHERE UserName = @username AND (Role = 'Admin' OR Role = 'Employee') AND Status = 1";
            }
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@username", username);

            var user = _dbContext.Connection.QueryFirstOrDefault<Account>(sql: sqlCommand, param: paramet);

            return user;
        }

        public Account GetByUserName(string username)
        {
            var sqlCommand = "SELECT * FROM Account WHERE UserName = @username AND Status = 1";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@username", username);

            var user = _dbContext.Connection.QueryFirstOrDefault<Account>(sql: sqlCommand, param: paramet);

            return user;
        }

        /// <summary>
        /// Tìm kiếm tài khoản theo số điện thoại
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <returns>Trả về thông tin tài khoản</returns>
        /// Created by: Nguyễn Văn Trúc(17/2/2024)
        public Account GetByPhoneNumber(string phoneNumber)
        {
            var sqlCommand = "SELECT * FROM Account WHERE PhoneNumber = @phoneNumber";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@phoneNumber", phoneNumber);

            var user = _dbContext.Connection.QueryFirstOrDefault<Account>(sql: sqlCommand, param: paramet);

            return user;
        }

        /// <summary>
        /// Tìm kiếm tài khoản theo email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Trả về thông tin tài khoản</returns>
        /// Created by: Nguyễn Văn Trúc(17/2/2024)
        public Account GetByEmail(string email)
        {
            var sqlCommand = "SELECT * FROM Account WHERE Email = @email";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@email", email);

            var user = _dbContext.Connection.QueryFirstOrDefault<Account>(sql: sqlCommand, param: paramet);

            return user;
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        /// <param name="accountNew">Tài khoản muốn cập nhật</param>
        /// <returns>trả về 1 nếu cập nhật thành công</returns>
        /// Created by: Nguyễn Văn Trúc(19/2/2024)
        public int UpdateAccount(Account accountNew)
        {
            var account = GetById(accountNew.AccountId);
            if (account != null)
            {
                var sqlCommand = "Proc_UpdateAccount";
                DynamicParameters paramet = new DynamicParameters();
                paramet.Add("m_AccessFailedCount", accountNew.AccessFailedCount);
                paramet.Add("m_ConcurrencyStamp", accountNew.ConcurrencyStamp);
                paramet.Add("m_LockoutEnabled", accountNew.LockoutEnabled);
                paramet.Add("m_SecurityStamp", accountNew.SecurityStamp);
                paramet.Add("m_RefreshToken", accountNew.RefreshToken);
                paramet.Add("m_RefreshTokenExpiryTime", accountNew.RefreshTokenExpiryTime);
                paramet.Add("m_AccountId", accountNew.AccountId);

                var user = _dbContext.Connection.Query<Account>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);
            }
            return 1;
        }

        /// <summary>
        /// Thêm thông tin tài khoản
        /// </summary>
        /// <param name="account">tài khoản muốn thêm</param>
        /// <returns>Trả về 1 nếu thêm thành công</returns>
        /// Created by: Nguyễn Văn Trúc(19/2/2024)
        public int InsertAccount(Account account)
        {
            var sqlCommand = "Proc_InsertAccount";
            DynamicParameters paramet = new DynamicParameters();

            var props = typeof(Account).GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(account, null);
                paramet.Add($"m_{propName}", propValue);
            }
            var user = _dbContext.Connection.Query<Account>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);
            return 1;
        }

        /// <summary>
        /// Đăng nhâp
        /// </summary>
        /// <param name="account">Tài khoản dùng để đăng nhập</param>
        /// <returns>Trả về 1 nếu đăng nhập thành công</returns>
        /// Created by: Nguyễn Văn Trúc(19/2/2024)
        public Account Login(AccountLogin account, string role)
        {
            var user = GetByUserNameRole(account.Account, role);
            return user;
        }

        /// <summary>
        /// Update lại mật khẩu
        /// </summary>
        /// <param name="accountId">Id tài khoản</param>
        /// <param name="account">Tài khoản</param>
        /// <returns>Trả về 1 nếu update thành công</returns>
        /// Created by: Nguyễn Văn Trúc(9/3/2024)
        public int UpdatePassword(Guid accountId, Account account)
        {
            base.Update(account, account.AccountId);
            return 1;
        }

        public IEnumerable<Account> GetByRole(string role)
        {
            var sqlCommand = "SELECT * FROM Account WHERE Role = @role";

            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@role", role);

            var users = _dbContext.Connection.Query<Account>(sql: sqlCommand, param: paramet);

            return users;
        }

        public IEnumerable<Account> GetPaging(int pageSize, int pageIndex, string text, string role)
        {
            var sqlCommand = $"SELECT * FROM Account WHERE (FullName LIKE @text OR AccountCode LIKE @text) AND Role = @role ORDER BY COALESCE(ModifiedDate, CreatedDate) DESC";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);
            paramet.Add("@role", role);

            var entities = _dbContext.Connection.Query<Account>(sql: sqlCommand, param: paramet);
            entities = entities.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return entities;
        }

        public IEnumerable<Account> GetByTextUser(string text, string role)
        {
            var sqlCommand = $"SELECT * FROM Account WHERE (FullName LIKE @text OR AccountCode LIKE @text) AND Role = @role ";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);
            paramet.Add("@role", role);

            var entities = _dbContext.Connection.Query<Account>(sql: sqlCommand, param: paramet);

            return entities;
        }

        public string GetByUsername(string username)
        {
            var sqlCommand = $"SELECT * FROM Account WHERE UserName = @name";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@name", username);

            var entity = _dbContext.Connection.QueryFirstOrDefault<Account>(sql: sqlCommand, param: paramet);

            if (entity != null)
            {
                return entity.Password.ToString();
            }
            return "";
        }
    }
}
