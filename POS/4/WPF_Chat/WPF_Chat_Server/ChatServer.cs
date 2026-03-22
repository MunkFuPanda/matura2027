using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ChatShared;

namespace WPF_Chat_Server;

public class ClientConnection
{
    public Transfer<ProtocolMessage> Transfer { get; set; } = null!;
    public string? Username { get; set; }
    public HashSet<string> JoinedRooms { get; set; } = [];
}

public class ChatServer
{
    private TcpListener? _listener;
    private readonly ConcurrentDictionary<string, ClientConnection> _clients = new();
    private readonly Database _db;
    private CancellationTokenSource? _cts;
    private readonly ConcurrentDictionary<string, string> _profileImageCache = new();

    public event Action<string>? OnLog;

    public ChatServer(Database db)
    {
        _db = db;
    }

    public async Task StartAsync(int port)
    {
        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Log($"Server started on port {port}");

        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync(_cts.Token);
                Log($"New connection from {tcpClient.Client.RemoteEndPoint}");
                _ = HandleClientAsync(tcpClient);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            Log($"Server error: {ex.Message}");
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
        foreach (var connection in _clients.Values)
        {
            try { connection.Transfer.Dispose(); } catch { }
        }
        _clients.Clear();
        _listener?.Stop();
        Log("Server stopped");
    }

    private async Task HandleClientAsync(TcpClient tcpClient)
    {
        var connection = new ClientConnection
        {
            Transfer = new Transfer<ProtocolMessage>(tcpClient)
        };

        connection.Transfer.MessageReceived += msg => ProcessMessageAsync(connection, msg);

        try
        {
            await connection.Transfer.Completion;
        }
        catch (Exception ex)
        {
            Log($"Client error ({connection.Username ?? "unknown"}): {ex.Message}");
        }
        finally
        {
            if (connection.Username != null)
            {
                _clients.TryRemove(connection.Username, out _);
                Log($"Disconnect: {connection.Username}");
                await BroadcastOnlineUsers();
            }
            try { connection.Transfer.Dispose(); } catch { }
        }
    }

    private async Task ProcessMessageAsync(ClientConnection connection, ProtocolMessage msg)
    {
        switch (msg.Type)
        {
            case MessageType.RegisterRequest:
                await HandleRegister(connection, msg);
                break;
            case MessageType.LoginRequest:
                await HandleLogin(connection, msg);
                break;
            case MessageType.ListRoomsRequest:
                await HandleListRooms(connection);
                break;
            case MessageType.CreateRoomRequest:
                await HandleCreateRoom(connection, msg);
                break;
            case MessageType.JoinRoomRequest:
                await HandleJoinRoom(connection, msg);
                break;
            case MessageType.LeaveRoomRequest:
                await HandleLeaveRoom(connection, msg);
                break;
            case MessageType.ChatMessage:
                await HandleChatMessage(connection, msg);
                break;
            case MessageType.PrivateMessage:
                await HandlePrivateMessage(connection, msg);
                break;
            case MessageType.UploadProfileImage:
                await HandleUploadProfileImage(connection, msg);
                break;
            case MessageType.RequestProfileImage:
                await HandleRequestProfileImage(connection, msg);
                break;
            case MessageType.UpdateColor:
                await HandleUpdateColor(connection, msg);
                break;
        }
    }

    private async Task HandleRegister(ClientConnection connection, ProtocolMessage msg)
    {
        bool success = _db.RegisterUser(msg.Username!, msg.Password!);
        Log($"Registration {(success ? "successful" : "failed")}: {msg.Username}");
        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.RegisterResponse,
            Success = success,
            ErrorMessage = success ? null : "Username already exists"
        });
    }

    private async Task HandleLogin(ClientConnection connection, ProtocolMessage msg)
    {
        if (_clients.ContainsKey(msg.Username!))
        {
            await SendAsync(connection, new ProtocolMessage
            {
                Type = MessageType.LoginResponse,
                Success = false,
                ErrorMessage = "User already logged in"
            });
            return;
        }

        bool valid = _db.ValidateUser(msg.Username!, msg.Password!);
        if (valid)
        {
            connection.Username = msg.Username;
            _clients[msg.Username!] = connection;
            string color = _db.GetUserColor(msg.Username!);
            Log($"Login: {msg.Username}");
            await SendAsync(connection, new ProtocolMessage
            {
                Type = MessageType.LoginResponse,
                Success = true,
                Username = msg.Username,
                Color = color
            });
            await BroadcastOnlineUsers();
        }
        else
        {
            Log($"Login failed: {msg.Username}");
            await SendAsync(connection, new ProtocolMessage
            {
                Type = MessageType.LoginResponse,
                Success = false,
                ErrorMessage = "Invalid username or password"
            });
        }
    }

    private async Task HandleListRooms(ClientConnection connection)
    {
        var rooms = _db.GetRooms();
        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.ListRoomsResponse,
            Rooms = rooms
        });
    }

    private async Task HandleCreateRoom(ClientConnection connection, ProtocolMessage msg)
    {
        bool success = _db.CreateRoom(msg.RoomName!, connection.Username!);
        Log($"Room {(success ? "created" : "creation failed")}: {msg.RoomName} by {connection.Username}");
        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.CreateRoomResponse,
            Success = success,
            RoomName = msg.RoomName,
            ErrorMessage = success ? null : "Room already exists"
        });

        if (success)
        {
            var rooms = _db.GetRooms();
            foreach (var client in _clients.Values)
            {
                await SendAsync(client, new ProtocolMessage
                {
                    Type = MessageType.ListRoomsResponse,
                    Rooms = rooms
                });
            }
        }
    }

    private async Task HandleJoinRoom(ClientConnection connection, ProtocolMessage msg)
    {
        connection.JoinedRooms.Add(msg.RoomName!);
        Log($"{connection.Username} joined room: {msg.RoomName}");

        var messages = _db.GetMessages(msg.RoomName!);
        var messageData = messages.Select(m => new ChatMessageData
        {
            FromUsername = m.From,
            Content = m.Content,
            Timestamp = m.Timestamp,
            Color = _db.GetUserColor(m.From)
        }).ToList();

        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.JoinRoomResponse,
            Success = true,
            RoomName = msg.RoomName,
            Messages = messageData
        });
    }

    private async Task HandleLeaveRoom(ClientConnection connection, ProtocolMessage msg)
    {
        connection.JoinedRooms.Remove(msg.RoomName!);
        Log($"{connection.Username} left room: {msg.RoomName}");
        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.LeaveRoomResponse,
            Success = true,
            RoomName = msg.RoomName
        });
    }

    private async Task HandleChatMessage(ClientConnection connection, ProtocolMessage msg)
    {
        var timestamp = DateTime.Now;
        _db.SaveMessage(msg.RoomName!, connection.Username!, msg.Content!, timestamp);
        Log($"[{msg.RoomName}] {connection.Username}: {msg.Content}");

        string color = _db.GetUserColor(connection.Username!);
        var broadcast = new ProtocolMessage
        {
            Type = MessageType.ChatMessageBroadcast,
            RoomName = msg.RoomName,
            FromUsername = connection.Username,
            Content = msg.Content,
            Color = color,
            Timestamp = timestamp
        };

        foreach (var client in _clients.Values)
        {
            if (client.JoinedRooms.Contains(msg.RoomName!))
            {
                await SendAsync(client, broadcast);
            }
        }
    }

    private async Task HandlePrivateMessage(ClientConnection connection, ProtocolMessage msg)
    {
        var timestamp = DateTime.Now;
        _db.SavePrivateMessage(connection.Username!, msg.ToUsername!, msg.Content!, timestamp);
        Log($"PM {connection.Username} -> {msg.ToUsername}: {msg.Content}");

        string color = _db.GetUserColor(connection.Username!);
        var pmMsg = new ProtocolMessage
        {
            Type = MessageType.PrivateMessageReceived,
            FromUsername = connection.Username,
            ToUsername = msg.ToUsername,
            Content = msg.Content,
            Color = color,
            Timestamp = timestamp
        };

        if (_clients.TryGetValue(msg.ToUsername!, out var recipient))
        {
            await SendAsync(recipient, pmMsg);
        }
        await SendAsync(connection, pmMsg);
    }

    private async Task HandleUploadProfileImage(ClientConnection connection, ProtocolMessage msg)
    {
        _db.UpdateProfileImage(connection.Username!, msg.ImageBase64!);
        _profileImageCache[connection.Username!] = msg.ImageBase64!;
        Log($"Profile image updated: {connection.Username}");
        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.UploadProfileImageResponse,
            Success = true
        });
    }

    private async Task HandleRequestProfileImage(ClientConnection connection, ProtocolMessage msg)
    {
        string? image;
        if (!_profileImageCache.TryGetValue(msg.Username!, out image))
        {
            image = _db.GetProfileImage(msg.Username!);
            if (image != null)
                _profileImageCache[msg.Username!] = image;
        }
        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.ProfileImageData,
            Username = msg.Username,
            ImageBase64 = image
        });
    }

    private async Task HandleUpdateColor(ClientConnection connection, ProtocolMessage msg)
    {
        _db.UpdateUserColor(connection.Username!, msg.Color!);
        Log($"Color updated: {connection.Username} -> {msg.Color}");
        await SendAsync(connection, new ProtocolMessage
        {
            Type = MessageType.UpdateColorResponse,
            Success = true,
            Color = msg.Color
        });
    }

    private async Task BroadcastOnlineUsers()
    {
        var users = _clients.Keys.ToList();
        var msg = new ProtocolMessage
        {
            Type = MessageType.OnlineUsers,
            Users = users
        };
        foreach (var client in _clients.Values)
        {
            try { await SendAsync(client, msg); } catch { }
        }
    }

    private async Task SendAsync(ClientConnection connection, ProtocolMessage msg)
    {
        try
        {
            await connection.Transfer.SendAsync(msg);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Log($"Send error to {connection.Username}: {ex.Message}");
        }
    }

    private void Log(string message)
    {
        OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
    }
}
