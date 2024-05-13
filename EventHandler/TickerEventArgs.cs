using Newtonsoft.Json;

using ShareInvest.UPbit.Models;

namespace ShareInvest.UPbit.EventHandler;

public class TickerEventArgs(string json) : EventArgs
{
    public RealTicker? Ticker
    {
        get;
    }
        = JsonConvert.DeserializeObject<RealTicker>(json);
}