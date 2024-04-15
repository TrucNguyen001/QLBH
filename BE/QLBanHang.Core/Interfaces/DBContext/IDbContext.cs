using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.DBContext
{
    public interface IDbContext
    {
        IDbConnection Connection { get; }
    }
}
