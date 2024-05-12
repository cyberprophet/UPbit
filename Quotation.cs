using Newtonsoft.Json;

using RestSharp;

using ShareInvest.UPbit.Models;

using System.Net;
using System.Text;

namespace ShareInvest.UPbit;

public class Quotation : RestClient
{
    public Quotation() : base("https://api.upbit.com/v1")
    {

    }

    public async Task<RestResponse> GetMarketAsync(bool isDetails = true)
    {
        return await ExecuteAsync(new RestRequest($"market/all?isDetails={isDetails}"), cts.Token);
    }

    public async Task<Market[]> GetMarketAsync()
    {
        var res = await GetMarketAsync(true);

        if (HttpStatusCode.OK != res.StatusCode || string.IsNullOrEmpty(res.Content))
        {
            return [];
        }
        return JsonConvert.DeserializeObject<Market[]>(res.Content) ?? [];
    }

    public async Task<RestResponse> GetTickerAsync(params string[] codeArr)
    {
        StringBuilder sb = new();

        foreach (string code in codeArr)
        {
            sb.Append(code);
            sb.Append(',');
        }
        var request = new RestRequest($"ticker?markets={sb.Remove(sb.Length - 1, 1)}");

        return await ExecuteAsync(request, cts.Token);
    }

    public async Task<Ticker[]> GetTickerAsync(Market[] markets)
    {
        var res = await GetTickerAsync([.. from m in markets
                                           where !string.IsNullOrEmpty(m.Code)
                                           select m.Code]);

        if (HttpStatusCode.OK != res.StatusCode || string.IsNullOrEmpty(res.Content))
        {
            return [];
        }
        return JsonConvert.DeserializeObject<Ticker[]>(res.Content) ?? [];
    }

    readonly CancellationTokenSource cts = new();
}