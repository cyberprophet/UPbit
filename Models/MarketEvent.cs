namespace ShareInvest.UPbit.Models;

public class MarketEvent
{
    public bool Warning
    {
        get; set;
    }

    public Caution? Caution
    {
        get; set;
    }
}