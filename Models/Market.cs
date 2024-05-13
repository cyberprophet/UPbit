using Newtonsoft.Json;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ShareInvest.UPbit.Models;

public class Market
{
    /// <summary>업비트에서 제공중인 시장 정보</summary>
    [DataMember, JsonProperty("market"), JsonPropertyName("market")]
    public string? Code
    {
        get; set;
    }

    [DataMember, JsonProperty("korean_name"), JsonPropertyName("korean_name")]
    public string? KoreanName
    {
        get; set;
    }

    [DataMember, JsonProperty("english_name"), JsonPropertyName("english_name")]
    public string? EnglishName
    {
        get; set;
    }

    [DataMember, JsonProperty("market_event"), JsonPropertyName("market_event")]
    public MarketEvent? Event
    {
        get; set;
    }
}