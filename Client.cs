using System.Text.Json;
using System.Text.Json.Serialization;

public class Client
{
    private string filePath;

    public Client(string filePath)
    {
        this.filePath = filePath;
    }

    public string Init()
    {
        // Generate a random secret string
        string secret = SecretKey.GenerateRandomString(32);

        // Create a JSON object with the secret
        var secretObject = new Dictionary<string, string> { { "SecretKey", secret } };
        var json = JsonSerializer.Serialize(secretObject);

        // Write the JSON to the filePath, overwrite if already exists
        File.WriteAllText(filePath, json);
        // return the secret
        return secret;
    }

    public string GetSecretKey()
    {
        string json = File.ReadAllText(filePath);
        var secretObject = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        if (secretObject == null || !secretObject.TryGetValue("SecretKey", out var secret))
        {
            throw new ArgumentException("SecretKey not found or deserialization failed.");
        }
        return secret;
    }
}
