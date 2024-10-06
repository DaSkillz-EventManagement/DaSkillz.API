using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public string? EventName { get; set; }
        public string? Location { get; set; }
        public string? Hashtag { get; set; }
        public long CreatedDate { get; set; }

        
    }
}
