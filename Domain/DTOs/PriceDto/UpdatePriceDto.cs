using Domain.Enum.Price;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.PriceDto;

public class UpdatePriceDto
{
    [Required(ErrorMessage = "PriceId is required!")]
    public int PriceId { get; set; }
    public PriceType PriceType { get; set; } //ex: advertisement, premium, ticket,...

    [Range(0, 500000000, ErrorMessage = "Maximum price amount is 500 000 000")]
    public double amount { get; set; } = 100000;
    [MaxLength(5000, ErrorMessage = "note max length is 5000 characters!")]
    public string? note { get; set; }
    [Required(ErrorMessage = "unit is required!")]
    [MaxLength(200, ErrorMessage = "unit max length is 200 characters!")]
    public string unit { get; set; } = string.Empty;
}
