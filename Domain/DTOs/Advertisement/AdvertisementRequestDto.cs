using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AdvertisementRequestDto
    {
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public string Status { get; set; }
    }
}
