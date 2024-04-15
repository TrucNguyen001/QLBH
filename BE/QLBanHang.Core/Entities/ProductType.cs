using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class ProductType
    {
        public Guid ProductTypeId { get; set; }

        public string ProductTypeCode {  get; set; }

        public string ProductTypeName { get; set; }

        public int Status {  get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
