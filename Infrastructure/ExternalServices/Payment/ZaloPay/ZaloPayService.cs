using Application.Abstractions.Payment.ZaloPay;
using Application.Helper.ZaloPayHelper;
using Application.Helper.ZaloPayHelper.Crypto;
using Domain.Constants.Domain;
using Infrastructure.ExternalServices.Payment.ZaloPay.Setting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ZaloPay.Helper;

namespace Infrastructure.ExternalServices.Payment.ZaloPay
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly ZaloPaySetting _zaloPaySettings;

        public ZaloPayService(IOptions<ZaloPaySetting> zaloPaySettings)
        {
            _zaloPaySettings = zaloPaySettings.Value;
        }

        //public async Task<APIResponse> GetBankListAsync()
        //{
        //    var reqtime = Utils.GetTimeStamp().ToString();

        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("appid", _zaloPaySettings.Appid!);
        //    param.Add("reqtime", reqtime);
        //    param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key1!, _zaloPaySettings.Appid! + "|" + reqtime));

        //    var result = await HttpHelper.PostFormAsync<BankListResponse>(_zaloPaySettings.GetBankListUrl!, param);

        //    if (result.returncode != "1")
        //    {
        //        return new APIResponse
        //        {
        //            StatusResponse = (HttpStatusCode)result.returncode,
        //            Message = result.returnmessage
        //        };
        //    }

        //    return new APIResponse
        //    {
        //        StatusResponse = result.returncode,
        //        Message = result.returnmessage,
        //        Data = result.
        //    };
        //}


        public async Task<Dictionary<string, object>> CreateOrderAsync(string amount, string appUser, string description, string app_trans_id, string redirectUrl)
        {
            var redirect = $"{DomainName.PRODUCTION_URL}?url={redirectUrl}&apptransid={app_trans_id}";
            var embed_data = new { redirecturl = redirect };
            var items = new[] { new { } };
            var param = new Dictionary<string, string>();

            param.Add("app_id", _zaloPaySettings.Appid!);
            param.Add("app_user", appUser);
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("expire_duration_seconds", "604800");
            param.Add("amount", amount);
            param.Add("app_trans_id", app_trans_id);
            param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("callback_url", _zaloPaySettings.CallbackUrl!);
            param.Add("description", description + app_trans_id);
            param.Add("bank_code", "");

            var data = _zaloPaySettings.Appid + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                        + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key1, data));

            var result = await HttpHelper.PostFormAsync(_zaloPaySettings.CreateOrderUrl, param);
            return result;
        }

        public bool ValidateMac(string dataStr, string reqMac)
        {
            try
            {

                var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key2, dataStr);
                return reqMac.Equals(mac);
            }
            catch
            {
                return false;
            }
        }

        public Dictionary<string, object> DeserializeData(string dataStr)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
        }

        public async Task<Dictionary<string, object>> QueryOrderStatus(string appTransId)
        {
            var param = new Dictionary<string, string>
                {
                    { "app_id", _zaloPaySettings.Appid! },
                    { "app_trans_id", appTransId}
                };

            var data = $"{_zaloPaySettings.Appid}|{appTransId}|{_zaloPaySettings.Key1}";
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, $"{_zaloPaySettings.Key1}", data));

            return await HttpHelper.PostFormAsync("https://sb-openapi.zalopay.vn/v2/query", param);
        }

        public async Task<Dictionary<string, object>> Refund(string zpTransId, string amount, string description)
        {
            var timestamp = Utils.GetTimeStamp().ToString();
            var rand = new Random();
            var uid = timestamp + "" + rand.Next(111, 999).ToString();

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("app_id", _zaloPaySettings.Appid!);
            param.Add("m_refund_id", DateTime.Now.ToString("yyMMdd") + "_" + _zaloPaySettings.Appid! + "_" + uid);
            param.Add("zp_trans_id", zpTransId);
            param.Add("amount", amount);
            param.Add("timestamp", timestamp);
            param.Add("description", description);

            var data = _zaloPaySettings.Appid! + "|" + param["zp_trans_id"] + "|" + param["amount"] + "|" + param["description"] + "|" + param["timestamp"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key1!, data));

            var result = await HttpHelper.PostFormAsync(_zaloPaySettings.RefundUrl!, param);
            return result;
        }

    }
}
