using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class EventLocation
    {
        [Required(ErrorMessage = "Location is required!")]
        [MaxLength(500, ErrorMessage = "Location must be between 3 and 250 characters!")]
        [MinLength(5, ErrorMessage = "Location must be between 3 and 250 characters!")]
        public string Name { get; set; } = null;


        [Required(ErrorMessage = "LocationId is required!")]
        public string? Id { get; set; } = null;


        [Required(ErrorMessage = "LocationAddress is required!")]
        public string? Address { get; set; } = null;

        [Required(ErrorMessage = "LocationUrl is required!")]
        public string? Url { get; set; } = null;

        [Required(ErrorMessage = "LocationCoord is required!")]
        public string? Coord { get; set; } = null;
    }
}
