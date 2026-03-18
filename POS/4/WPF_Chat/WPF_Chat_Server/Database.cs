using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using System.Security.Cryptography;
using System.Text;

namespace WPF_Chat_Server;

[Table("Users")]
public class UserEntity
{
    [PrimaryKey, Identity]
    public int Id { get; set; }

    [Column, NotNull]
    public string Username { get; set; } = "";

    [Column, NotNull]
    public string PasswordHash { get; set; } = "";

    [Column, NotNull]
    public string Color { get; set; } = "#FF000000";

    [Column(CanBeNull = true)]
    public string? ProfileImageBase64 { get; set; }
}

[Table("ChatRooms")]
public class ChatRoomEntity
{
    [PrimaryKey, Identity]
    public int Id { get; set; }

    [Column, NotNull]
    public string Name { get; set; } = "";

    [Column, NotNull]
    public string CreatedBy { get; set; } = "";
}

[Table("Messages")]
public class MessageEntity
{
    [PrimaryKey, Identity]
    public int Id { get; set; }

    [Column, NotNull]
    public string RoomName { get; set; } = "";

    [Column, NotNull]
    public string FromUsername { get; set; } = "";

    [Column, NotNull]
    public string Content { get; set; } = "";

    [Column, NotNull]
    public string Timestamp { get; set; } = "";
}

[Table("PrivateMessages")]
public class PrivateMessageEntity
{
    [PrimaryKey, Identity]
    public int Id { get; set; }

    [Column, NotNull]
    public string FromUsername { get; set; } = "";

    [Column, NotNull]
    public string ToUsername { get; set; } = "";

    [Column, NotNull]
    public string Content { get; set; } = "";

    [Column, NotNull]
    public string Timestamp { get; set; } = "";
}

public class ChatDb : DataConnection
{
    public ChatDb(string connectionString)
        : base(ProviderName.SQLiteMS, connectionString)
    {
    }

    public ITable<UserEntity> Users => this.GetTable<UserEntity>();
    public ITable<ChatRoomEntity> ChatRooms => this.GetTable<ChatRoomEntity>();
    public ITable<MessageEntity> Messages => this.GetTable<MessageEntity>();
    public ITable<PrivateMessageEntity> PrivateMessages => this.GetTable<PrivateMessageEntity>();
}

public class Database : IDisposable
{
    private readonly string _connectionString;

    public Database(string dbPath = "chat.db")
    {
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    private ChatDb CreateDb() => new(_connectionString);

    private void InitializeDatabase()
    {
        using var db = CreateDb();
        db.CreateTable<UserEntity>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<ChatRoomEntity>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<MessageEntity>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<PrivateMessageEntity>(tableOptions: TableOptions.CreateIfNotExists);

        try { db.Execute("CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Username ON Users(Username)"); } catch { }
        try { db.Execute("CREATE UNIQUE INDEX IF NOT EXISTS IX_ChatRooms_Name ON ChatRooms(Name)"); } catch { }
    }

    public bool RegisterUser(string username, string password)
    {
        try
        {
            using var db = CreateDb();
            db.Insert(new UserEntity
            {
                Username = username,
                PasswordHash = HashPassword(password)
            });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ValidateUser(string username, string password)
    {
        using var db = CreateDb();
        var hash = db.Users
            .Where(u => u.Username == username)
            .Select(u => u.PasswordHash)
            .FirstOrDefault();
        return hash != null && hash == HashPassword(password);
    }

    public List<string> GetRooms()
    {
        using var db = CreateDb();
        return db.ChatRooms
            .Select(r => r.Name)
            .ToList();
    }

    public bool CreateRoom(string name, string createdBy)
    {
        try
        {
            using var db = CreateDb();
            db.Insert(new ChatRoomEntity
            {
                Name = name,
                CreatedBy = createdBy
            });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void SaveMessage(string roomName, string fromUsername, string content, DateTime timestamp)
    {
        using var db = CreateDb();
        db.Insert(new MessageEntity
        {
            RoomName = roomName,
            FromUsername = fromUsername,
            Content = content,
            Timestamp = timestamp.ToString("o")
        });
    }

    public List<(string From, string Content, DateTime Timestamp)> GetMessages(string roomName, int limit = 100)
    {
        using var db = CreateDb();
        var messages = db.Messages
            .Where(m => m.RoomName == roomName)
            .OrderByDescending(m => m.Id)
            .Take(limit)
            .Select(m => new { m.FromUsername, m.Content, m.Timestamp })
            .ToList();

        messages.Reverse();
        return messages
            .Select(m => (m.FromUsername, m.Content, DateTime.Parse(m.Timestamp)))
            .ToList();
    }

    public void SavePrivateMessage(string from, string to, string content, DateTime timestamp)
    {
        using var db = CreateDb();
        db.Insert(new PrivateMessageEntity
        {
            FromUsername = from,
            ToUsername = to,
            Content = content,
            Timestamp = timestamp.ToString("o")
        });
    }

    public void UpdateProfileImage(string username, string base64Image)
    {
        using var db = CreateDb();
        db.Users
            .Where(u => u.Username == username)
            .Set(u => u.ProfileImageBase64, base64Image)
            .Update();
    }

    public string? GetProfileImage(string username)
    {
        using var db = CreateDb();
        return db.Users
            .Where(u => u.Username == username)
            .Select(u => u.ProfileImageBase64)
            .FirstOrDefault();
    }

    public void UpdateUserColor(string username, string color)
    {
        using var db = CreateDb();
        db.Users
            .Where(u => u.Username == username)
            .Set(u => u.Color, color)
            .Update();
    }

    public string GetUserColor(string username)
    {
        using var db = CreateDb();
        return db.Users
            .Where(u => u.Username == username)
            .Select(u => u.Color)
            .FirstOrDefault() ?? "#FF000000";
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public void Dispose()
    {
    }
}
