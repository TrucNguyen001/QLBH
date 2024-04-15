using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Interfaces.Infastructure
{
    public interface IProductTypeRepository : IBaseRepository<ProductType>
    {
        /// <summary>
        /// Hàm giúp sắp xếp danh sách giảm dần theo mã
        /// </summary>
        /// <returns>
        /// Trả về danh sách giảm dần theo mã
        /// </returns>
        /// CreateBy: NVTruc(2/4/2024)
        public IEnumerable<ProductType> SortDecrease();
    }
}
