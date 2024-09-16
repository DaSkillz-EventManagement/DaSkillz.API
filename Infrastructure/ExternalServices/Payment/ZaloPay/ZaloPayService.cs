using Application.Abstractions.Payment.ZaloPay;
using Application.Helper.ZaloPayHelper;
using Application.Helper.ZaloPayHelper.Crypto;
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


        public async Task<Dictionary<string, object>> CreateOrderAsync(string amount, string appUser, string description, string app_trans_id)
        {
            var embed_data = new { };
            var items = new[] { new { } };
            var param = new Dictionary<string, string>();

            param.Add("app_id", _zaloPaySettings.Appid!);
            param.Add("app_user", appUser);
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("amount", amount);
            param.Add("app_trans_id", app_trans_id);
            param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("callback_url", "");
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
            var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPaySettings.Key2, dataStr);
            return reqMac.Equals(mac);
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

        //public IActionResult CallBack([FromBody] dynamic cbdata)
        //{
        //    var result = new Dictionary<string, object>();

        //    try
        //    {
        //        var dataStr = Convert.ToString(cbdata["data"]);
        //        var reqMac = Convert.ToString(cbdata["mac"]);

        //        var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key2, dataStr);

        //        Console.WriteLine("mac = {0}", mac);

        //        // kiểm tra callback hợp lệ (đến từ ZaloPay server)
        //        if (!reqMac.Equals(mac))
        //        {
        //            // callback không hợp lệ
        //            result["return_code"] = -1;
        //            result["return_message"] = "mac not equal";
        //        }
        //        else
        //        {
        //            var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
        //            Console.WriteLine("update order's status = success where app_trans_id = {0}", dataJson["app_trans_id"]);

        //            result["return_code"] = 1;
        //            result["return_message"] = "success";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result["return_code"] = 0; // ZaloPay server sẽ callback lại (tối đa 3 lần)
        //        result["return_message"] = ex.Message;
        //    }

        //    // thông báo kết quả cho ZaloPay server
        //    return Ok(result);
        //}


        //truy vấn trạng thái đơn hàng
        private readonly string queryOrderUrl = "https://sb-openapi.zalopay.vn/v2/query";

        //[HttpGet("query-order")]
        //public async Task<IActionResult> QueryOrder()
        //{

        //    try
        //    {
        //        var param = new Dictionary<string, string>
        //        {
        //            { "app_id", "2553" },
        //            { "app_trans_id", "240910_153047" }
        //        };

        //        var data = $"{"2553"}|{"240910_153047"}|{"PcY4iZIKFCIdgZvA6ueMcMHHUbRLYjPL"}";
        //        param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, "PcY4iZIKFCIdgZvA6ueMcMHHUbRLYjPL", data));

        //        var result = await HttpHelper.PostFormAsync(queryOrderUrl, param);

        //        // Trả về kết quả truy vấn trạng thái đơn hàng
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý ngoại lệ và trả về lỗi
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}

    }
}
