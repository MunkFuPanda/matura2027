using ChatShared;

namespace WPF_Chat;

public class ChatClient : IDisposable
{
    private Transfer<ProtocolMessage>? _transfer;

    public string? Username { get; private set; }
    public string? UserColor { get; set; }
    public bool IsConnected => _transfer?.Connected ?? false;

    public event Action<ProtocolMessage>? OnMessageReceived;
    public event Action<string>? OnDisconnected;

    public async Task ConnectAsync(string host, int port)
    {
        _transfer = await Transfer<ProtocolMessage>.ConnectAsync(host, port);
        _transfer.MessageReceived += msg =>
        {
            OnMessageReceived?.Invoke(msg);
            return Task.CompletedTask;
        };
        _transfer.Disconnected += () => OnDisconnected?.Invoke("Disconnected");
    }

    public async Task<ProtocolMessage> RegisterAsync(string username, string password)
    {
        await SendAsync(new ProtocolMessage
        {
            Type = MessageType.RegisterRequest,
            Username = username,
            Password = password
        });
        return await WaitForResponseAsync(MessageType.RegisterResponse);
    }

    public async Task<ProtocolMessage> LoginAsync(string username, string password)
    {
        await SendAsync(new ProtocolMessage
        {
            Type = MessageType.LoginRequest,
            Username = username,
            Password = password
        });
        var response = await WaitForResponseAsync(MessageType.LoginResponse);
        if (response.Success)
        {
            Username = username;
            UserColor = response.Color;
        }
        return response;
    }

    public async Task SendAsync(ProtocolMessage msg)
    {
        if (_transfer != null)
            await _transfer.SendAsync(msg);
    }

    private async Task<ProtocolMessage> WaitForResponseAsync(MessageType expectedType, int timeoutMs = 5000)
    {
        var tcs = new TaskCompletionSource<ProtocolMessage>();

        void Handler(ProtocolMessage msg)
        {
            if (msg.Type == expectedType)
            {
                OnMessageReceived -= Handler;
                tcs.TrySetResult(msg);
            }
        }

        OnMessageReceived += Handler;

        var timeout = Task.Delay(timeoutMs);
        var completed = await Task.WhenAny(tcs.Task, timeout);
        if (completed == timeout)
        {
            OnMessageReceived -= Handler;
            return new ProtocolMessage { Success = false, ErrorMessage = "Request timed out" };
        }
        return await tcs.Task;
    }

    public void Disconnect()
    {
        _transfer?.Dispose();
        _transfer = null;
    }

    public void Dispose()
    {
        Disconnect();
    }
}
