using Microsoft.Extensions.Configuration;
using MySqlConnector;
using QLBanHang.Core.Interfaces.DBContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Infrastructure.DatabaseContext
{
    public class MySqlDbContext : IDbContext
    {
        public IDbConnection Connection { get; }

        public MySqlDbContext(IConfiguration config)
        {
            Connection = new MySqlConnection(config.GetConnectionString("Database"));
        }
    }
}
