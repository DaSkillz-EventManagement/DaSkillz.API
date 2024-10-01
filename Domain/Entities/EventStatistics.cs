using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class EventStatistics
    {
        public Guid EventId { get; set; }
        public int TotalVisitor { get; set; }
        public int TotalCheckinStaff { get; set; }
        public int TotalSponsor { get; set; }
        public int TotalCheckedIn { get; set; }
        public int TotalCheckedMail { get; set; }
        public int TotalFeedback { get; set; }
        public decimal AverageStar { get; set; }
        public int TotalFbOneStar { get; set; }
        public int TotalFbTwoStar { get; set; }
        public int TotalFbThreeStar { get; set; }
        public int TotalFbFourStar { get; set; }
        public int TotalFbFiveStar { get; set; }
        public decimal TotalSponsored { get; set; }

        public virtual Event Event { get; set; }
    }
}
