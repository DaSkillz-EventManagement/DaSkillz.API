namespace Infrastructure.ExternalServices.Payment.ZaloPay.Setting
{
    public class ZaloPaySetting
    {
        public string? Appid { get; set; }
        public string? Key1 { get; set; }
        public string? Key2 { get; set; }
        public string? GetBankListUrl { get; set; }
        public string? CreateOrderUrl { get; set; }
        public string? QueryOrderUrl { get; set; }
        public string? RefundUrl { get; set; }
        public string? QueryRefundUrl { get; set; }
    }

}
