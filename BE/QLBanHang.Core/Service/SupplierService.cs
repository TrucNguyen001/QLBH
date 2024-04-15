using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Service
{
    public class SupplierService : BaseService<Supplier>, ISupplierService
    {
        ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository) : base(supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public string GetSupplierCodeBiggest()
        {
            var suppliers = _supplierRepository.SortDecrease();
            Supplier supplier = suppliers.FirstOrDefault();
            var supplierCode = supplier.SupplierCode;
            var numberCode = supplierCode.Substring(3);
            var numberCodeBiggest = Convert.ToInt64(numberCode) + 1;
            numberCode = PadLeftCustom(Convert.ToString(numberCodeBiggest), numberCode.Length, '0');
            supplierCode = supplierCode.Substring(0, 3) + numberCode;
            return supplierCode;
        }
    }
}
