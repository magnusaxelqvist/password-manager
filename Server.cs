using System.Text.Json;

public class Server
{
    private string filePath;

    public Server(string filePath)
    {
        this.filePath = filePath;
    }

    public void Init(string masterPassword, string secretKey)
    {
        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Crypto.GenerateIv();

        Dictionary<string, string> serverDictionary = new Dictionary<string, string>
        {
            {"iv", Convert.ToBase64String(vaultIv)},
            {"vault", Convert.ToBase64String(Crypto.EncryptVault(new Vault(), vaultKey, vaultIv))}
        };

        SaveServerDictionaryToFile(this.filePath, serverDictionary);
    }

    public string? GetProperty(string masterPassword, string secretKey, string property)
    {
        var serverDictionary = ReadServerDictionaryFromFile(this.filePath);
        Vault vault = GetVaultFromServerDictionary(serverDictionary, masterPassword, secretKey);
        return vault.Get(property);
    }

    public void SetProperty(string masterPassword, string secretKey, string property, string value)
    {
        var serverDictionary = ReadServerDictionaryFromFile(this.filePath);
        Vault vault = GetVaultFromServerDictionary(serverDictionary, masterPassword, secretKey);
        vault.Set(property, value);

        SetVaultInServerDictionary(serverDictionary, vault, masterPassword, secretKey);
        SaveServerDictionaryToFile(this.filePath, serverDictionary);
    }

    private Dictionary<string, string> ReadServerDictionaryFromFile(string filePath)
    {
        string serverJson = File.ReadAllText(filePath);
        var serverDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(serverJson) ?? throw new InvalidOperationException("Failed to deserialize the server dictionary from file.");
        return serverDictionary;
    }

    private void SaveServerDictionaryToFile(string filePath, Dictionary<string, string> serverDictionary)
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(serverDictionary));
    }

    private Vault GetVaultFromServerDictionary(Dictionary<string, string> serverDictionary, string masterPassword, string secretKey)
    {
        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Convert.FromBase64String(serverDictionary["iv"]);
        byte[] encryptedVault = Convert.FromBase64String(serverDictionary["vault"]);

        return Crypto.DecryptVault(encryptedVault, vaultKey, vaultIv);
    }

    private void SetVaultInServerDictionary(Dictionary<string, string> serverDictionary, Vault vault, string masterPassword, string secretKey)
    {
        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Convert.FromBase64String(serverDictionary["iv"]);
        serverDictionary["vault"] = Convert.ToBase64String(Crypto.EncryptVault(vault, vaultKey, vaultIv));
    }
}
