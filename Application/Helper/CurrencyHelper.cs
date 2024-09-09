using Newtonsoft.Json.Linq;

namespace Application.Helper
{
    public static class CurrencyHelper
    {
        public static async Task<string> GetExchangeRate(string url, decimal? amount)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(responseBody);


                decimal exchangeRate = data["data"]["VND"]["value"].Value<decimal>();

                // Tính toán số tiền sau khi chuyển đổi
                decimal translatedAmount = (decimal)amount / exchangeRate;
                return translatedAmount.ToString("F2");
            }
        }
    }
}
