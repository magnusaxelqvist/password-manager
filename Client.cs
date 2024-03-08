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
        // Generate a random secret string
        string secret = Crypto.GenerateSecretKey(32);

        FileDictionary dictionary = new FileDictionary(filePath);
        dictionary.Set("SecretKey", secret);
        dictionary.Save();

        return secret;
    }

    public string GetSecretKey()
    {
        FileDictionary dictionary = new FileDictionary(filePath).Load();

        string? secret = dictionary.Get("SecretKey");

        if (secret == null)
        {
            throw new ArgumentException("SecretKey not found or deserialization failed.");
        }
        return secret;
    }
}
