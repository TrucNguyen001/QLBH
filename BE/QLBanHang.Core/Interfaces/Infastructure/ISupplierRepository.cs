using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface ISupplierRepository : IBaseRepository<Supplier>
    {
        /// <summary>
        /// Hàm giúp sắp xếp danh sách nhân viên giảm dần theo mã
        /// </summary>
        /// <returns>
        /// Trả về danh sách nhân viên giảm dần theo mã
        /// </returns>
        /// CreateBy: NVTruc(28/12/2023)
        public IEnumerable<Supplier> SortDecrease();
    }
}
