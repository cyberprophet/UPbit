```C#
Ticker tickers;

using (var api = new Quotation())
{
    var market = await api.GetMarketAsync();

    tickers = await api.GetTickerAsync(market);

    foreach (var ticker in tickers)
    {
        Console.WriteLine(ticker.Code);
    }
}
using (var socket = new WebSocket())
{
    socket.SendTicker += (sender, e) =>
    {
        Console.WriteLine(JsonConvert.SerializeObject(e.Ticker, Formatting.Indented));
    };
    await socket.ConnectAsync();

    var task = Task.Run(socket.ReceiveAsync);

    await socket.RequestAsync(new
    {
        ticket = Guid.NewGuid()
    },
    new
    {
        type = "ticker",
        codes = tickers.Select(e => e.Code)
    });
    await task;
}
}
```
