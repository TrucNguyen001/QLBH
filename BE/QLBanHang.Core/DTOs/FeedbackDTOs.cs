using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.DTOs
{
    public class FeedbackDTOs : Comment
    {
        public Guid FeedbackId { get; set; }

        public string? ProductCode { get; set; }

        public string? ProductName { get; set; }

        public string FeedbackContent { get; set; }

        public DateTime? CreatedDateFeedback { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public string? FeedbackFor { get; set; }
    }
}
