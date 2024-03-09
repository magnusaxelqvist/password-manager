using System.Text.Json;

public class Client
{
    private string filePath;

    public Client(string filePath)
    {
        this.filePath = filePath;
    }

    public string Init()
    {
        string secret = Crypto.GenerateSecret(32);
        return Init(secret);
    }

    public string Init(string secretKey)
    {
        FileDictionary dictionary = new FileDictionary(filePath);
        dictionary.Set("secret", secretKey);
        dictionary.Save();

        return secretKey;
    }

    public string GetSecret()
    {
        FileDictionary dictionary = new FileDictionary(filePath).Load();

        string? secret = dictionary.Get("secret");

        if (secret == null)
        {
            throw new ArgumentException("SecretKey not found or deserialization failed.");
        }
        return secret;
    }
}
