using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }

        public string? CommentCode { get; set; }

        public string CommentName { get; set; }

        public string Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Guid ProductId { get; set; }

        public Guid AccountId { get; set; }

        public int? Status { get; set; }
    }
}
