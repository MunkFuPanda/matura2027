using System.Text.Json;

namespace ChatShared;

public enum MessageType
{
    RegisterRequest,
    RegisterResponse,
    LoginRequest,
    LoginResponse,
    ListRoomsRequest,
    ListRoomsResponse,
    CreateRoomRequest,
    CreateRoomResponse,
    JoinRoomRequest,
    JoinRoomResponse,
    LeaveRoomRequest,
    LeaveRoomResponse,
    ChatMessage,
    ChatMessageBroadcast,
    PrivateMessage,
    PrivateMessageReceived,
    UploadProfileImage,
    UploadProfileImageResponse,
    RequestProfileImage,
    ProfileImageData,
    UpdateColor,
    UpdateColorResponse,
    OnlineUsers,
    Error
}

public class ProtocolMessage
{
    public MessageType Type { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? RoomName { get; set; }
    public string? Content { get; set; }
    public string? ToUsername { get; set; }
    public string? FromUsername { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ImageBase64 { get; set; }
    public string? Color { get; set; }
    public List<string>? Rooms { get; set; }
    public List<ChatMessageData>? Messages { get; set; }
    public List<string>? Users { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public string Serialize() => JsonSerializer.Serialize(this);
    public static ProtocolMessage? Deserialize(string json) => JsonSerializer.Deserialize<ProtocolMessage>(json);
}

public class ChatMessageData
{
    public string? FromUsername { get; set; }
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Color { get; set; }
}
