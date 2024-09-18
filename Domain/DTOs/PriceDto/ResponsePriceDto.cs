using Domain.DTOs.User.Response;
using Domain.Enum.Price;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.PriceDto;

public class ResponsePriceDto
{
    public int PriceId { get; set; }
    public string PriceType { get; set; } = string.Empty;//ex: advertisement, premium, ticket,...
    public double amount { get; set; }
    public string note { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CreatedByUserDto CreatedBy { get; set; }
}
