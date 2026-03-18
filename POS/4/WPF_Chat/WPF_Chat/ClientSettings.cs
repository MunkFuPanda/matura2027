using System.IO;
using System.Xml.Serialization;

namespace WPF_Chat;

public class ClientSettings
{
    public string ServerHost { get; set; } = "127.0.0.1";
    public int ServerPort { get; set; } = 5000;
    public string? LastUsername { get; set; }

    private static readonly string SettingsPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "client_settings.xml");

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(ClientSettings));
        using var writer = new StreamWriter(SettingsPath);
        serializer.Serialize(writer, this);
    }

    public static ClientSettings Load()
    {
        if (!File.Exists(SettingsPath))
            return new ClientSettings();

        try
        {
            var serializer = new XmlSerializer(typeof(ClientSettings));
            using var reader = new StreamReader(SettingsPath);
            return (ClientSettings?)serializer.Deserialize(reader) ?? new ClientSettings();
        }
        catch
        {
            return new ClientSettings();
        }
    }
}
