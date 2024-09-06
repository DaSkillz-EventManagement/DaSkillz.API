using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Sponsors
{
    public class SponsorLogoDto
    {
        public int LogoId { get; set; }
        public string? SponsorBrand { get; set; } = string.Empty;
        public string? LogoUrl { get; set; } = string.Empty;
    }
}
