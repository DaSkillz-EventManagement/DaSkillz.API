using Domain.Enum.Price;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.PriceDto;

public class PriceDto
{
    [Required(ErrorMessage = "PriceType is required!")]
    public PriceType PriceType { get; set; } //ex: advertisement, premium, ticket,...

    [Required(ErrorMessage = "PriceType is required!")]
    [Range(0, 500000000, ErrorMessage = "Maximum price amount is 500 000 000")]
    public double amount { get; set; }
    [MaxLength(5000, ErrorMessage = "note max length is 5000 characters!")]
    public string note { get; set; } = string.Empty;
}
