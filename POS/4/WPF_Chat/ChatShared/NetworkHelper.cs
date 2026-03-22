using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatShared;

public class Transfer<T> : IDisposable
{
    private readonly TcpClient _client;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private readonly CancellationTokenSource _cts = new();

    public bool Connected => _client.Connected;
    public Task Completion { get; }

    public event Func<T, Task>? MessageReceived;
    public event Action? Disconnected;

    public Transfer(TcpClient client)
    {
        _client = client;
        var stream = _client.GetStream();
        _reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        _writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };
        Completion = Task.Run(ReceiveLoopAsync);
    }

    public static async Task<Transfer<T>> ConnectAsync(string host, int port)
    {
        var client = new TcpClient();
        await client.ConnectAsync(host, port);
        return new Transfer<T>(client);
    }

    public async Task SendAsync(T message, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(message);
        await _sendLock.WaitAsync(cancellationToken);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _writer.WriteLineAsync(json);
        }
        finally
        {
            _sendLock.Release();
        }
    }

    private async Task ReceiveLoopAsync()
    {
        try
        {
            while (!_cts.IsCancellationRequested)
            {
                var line = await _reader.ReadLineAsync();
                if (line == null) break;

                var message = JsonSerializer.Deserialize<T>(line);
                if (message == null || MessageReceived == null) continue;

                foreach (var handler in MessageReceived.GetInvocationList().Cast<Func<T, Task>>())
                    await handler(message);
            }
        }
        catch
        {
        }
        finally
        {
            Disconnected?.Invoke();
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        try { _client.Close(); } catch { }
        _reader.Dispose();
        _writer.Dispose();
        _client.Dispose();
        _sendLock.Dispose();
        _cts.Dispose();
    }
}
