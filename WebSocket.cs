using Newtonsoft.Json;

using ShareInvest.UPbit.EventHandler;

using System.Net.WebSockets;
using System.Text;

namespace ShareInvest.UPbit;

public class WebSocket : IDisposable
{
    public event EventHandler<TickerEventArgs>? SendTicker;

    /// <summary>
    /// ticket: 수신하는 대상을 식별
    /// type: 수신하고 싶은 시세 정보를 나열하는 필드
    /// format: DEFAULT(기본형), SIMPLE(축약형)
    /// </summary>
    public async Task RequestAsync(params object[] objArr)
    {
        Queue<object> queue = new();

        foreach (var obj in objArr)
        {
            queue.Enqueue(obj);
        }
        var json = JsonConvert.SerializeObject(queue);

        await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(json)), WebSocketMessageType.Text, true, cts.Token);
    }

    public async Task ReceiveAsync()
    {
        while (WebSocketState.Open == socket.State)
        {
            var buffer = new byte[0x400];

            var res = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);

            SendTicker?.Invoke(this, new TickerEventArgs(Encoding.UTF8.GetString(buffer, 0, res.Count)));
        }
    }

    public async Task ConnectAsync(string? token = null)
    {
        if (string.IsNullOrEmpty(token) is false)
        {
            socket.Options.SetRequestHeader("Authorization", $"Bearer {token}");
        }
        socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(0xA);

        await socket.ConnectAsync(new Uri("wss://api.upbit.com/websocket/v1"), cts.Token);
    }

    public void Dispose()
    {
        socket.Dispose();

        GC.SuppressFinalize(this);
    }

    readonly ClientWebSocket socket = new();
    readonly CancellationTokenSource cts = new();
}