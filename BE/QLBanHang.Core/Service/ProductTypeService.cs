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
    public class ProductTypeService : BaseService<ProductType>, IProductTypeService
    {
        IProductTypeRepository _productTypeRepository;
        public ProductTypeService(IProductTypeRepository productTypeRepository) : base(productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }

        public string GetProductTypeCodeBiggest()
        {
            var productTypes = _productTypeRepository.SortDecrease();
            ProductType product = productTypes.FirstOrDefault();
            var productTypeCode = product.ProductTypeCode;
            var numberCode = productTypeCode.Substring(3);
            var numberCodeBiggest = Convert.ToInt64(numberCode) + 1;
            numberCode = PadLeftCustom(Convert.ToString(numberCodeBiggest), numberCode.Length, '0');
            productTypeCode = productTypeCode.Substring(0, 3) + numberCode;
            return productTypeCode;
        }
    }
}
