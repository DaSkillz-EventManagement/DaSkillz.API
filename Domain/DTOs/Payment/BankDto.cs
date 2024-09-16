namespace Domain.DTOs.Payment
{
    public class BankDto
    {
        public string? bankcode { get; set; }
        public string? name { get; set; }
        public int displayorder { get; set; }
        public int pmcid { get; set; }
    }
}
