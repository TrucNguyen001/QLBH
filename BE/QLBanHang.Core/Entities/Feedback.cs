using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Feedback
    {
        public Guid FeedbackId { get; set; }

        public string FeedbackContent {get; set; }

        public Guid CommentId { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedDateFeedback { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public string? FeedbackFor { get; set; }
    }
}
