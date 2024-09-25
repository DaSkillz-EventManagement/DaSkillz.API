namespace Application.Abstractions.Payment.ZaloPay
{
    public interface IZaloPayService
    {
        Task<Dictionary<string, object>> CreateOrderAsync(string amount, string appUser, string description, string app_trans_id, string redirectUrl);
        bool ValidateMac(string dataStr, string reqMac);
        Dictionary<string, object> DeserializeData(string dataStr);
        Task<Dictionary<string, object>> QueryOrderStatus(string appTransId);
        Task<Dictionary<string, object>> Refund(string zpTransId, string amount, string description);
    }
}
