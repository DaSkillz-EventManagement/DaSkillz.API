using Domain.DTOs.Payment;

namespace Domain.Models.Response
{
    public class BankListResponse
    {
        public string? returncode { get; set; }
        public string? returnmessage { get; set; }
        public Dictionary<string, List<BankDto>> banks { get; set; }
    }
}
