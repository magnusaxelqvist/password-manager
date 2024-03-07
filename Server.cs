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

        // Serialize the server dictionary to JSON
        string serverJson = JsonSerializer.Serialize(serverDictionary);

        // Write the JSON to the filePath, overwrite if already exists
        File.WriteAllText(filePath, serverJson);
    }

    public void Set(string masterPassword, string secretKey, string property, string value)
    {
        string serverJson = File.ReadAllText(filePath);
        var serverDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(serverJson);
        if (serverDictionary == null)
        {
            throw new InvalidOperationException("Failed to deserialize the server dictionary.");
        }

        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Convert.FromBase64String(serverDictionary["iv"]);
        byte[] encryptedVault = Convert.FromBase64String(serverDictionary["vault"]);

        Vault vault = Crypto.DecryptVault(encryptedVault, vaultKey, vaultIv);
        vault.Set(property, value);

        serverDictionary["vault"] = Convert.ToBase64String(Crypto.EncryptVault(vault, vaultKey, vaultIv));
        serverJson = JsonSerializer.Serialize(serverDictionary);

        File.WriteAllText(filePath, serverJson);
    }

    public string? Get(string masterPassword, string secretKey, string property)
    {
        string serverJson = File.ReadAllText(filePath);
        var serverDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(serverJson);
        if (serverDictionary == null)
        {
            throw new InvalidOperationException("Failed to deserialize the server dictionary.");
        }

        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Convert.FromBase64String(serverDictionary["iv"]);
        byte[] encryptedVault = Convert.FromBase64String(serverDictionary["vault"]);

        Vault vault = Crypto.DecryptVault(encryptedVault, vaultKey, vaultIv);
        return vault.Get(property);
    }
}
