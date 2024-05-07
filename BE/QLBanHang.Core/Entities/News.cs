using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class News
    {
        public Guid NewsId { get; set; }

        public string? NewsCode { get; set; }

        public string? NewsName { get; set; }

        public string? Content { get; set; }

        public string? Image {  get; set; }

        public string? Introduce { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
