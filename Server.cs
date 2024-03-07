using System.Security.Cryptography;
using System.Text;
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
        byte[] vaultKey = GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Aes.Create().IV;

        Dictionary<string, string> serverDictionary = new Dictionary<string, string>
        {
            {"iv", Convert.ToBase64String(vaultIv)},
            {"vault", Convert.ToBase64String(EncryptVault(new Vault(), vaultKey, vaultIv))}
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

        Console.WriteLine($"Current server dictionary: {JsonSerializer.Serialize(serverDictionary)}");

        byte[] vaultKey = GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Convert.FromBase64String(serverDictionary["iv"]);
        byte[] encryptedVault = Convert.FromBase64String(serverDictionary["vault"]);

        Vault vault = DecryptVault(encryptedVault, vaultKey, vaultIv);
        vault.Set(property, value);

        Console.WriteLine($"Vault content: {JsonSerializer.Serialize(vault)}");

        serverDictionary["vault"] = Convert.ToBase64String(EncryptVault(vault, vaultKey, vaultIv));
        serverJson = JsonSerializer.Serialize(serverDictionary);

        Console.WriteLine($"Updated server dictionary: {JsonSerializer.Serialize(serverDictionary)}");

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

        byte[] vaultKey = GenerateVaultKey(masterPassword, secretKey);
        byte[] vaultIv = Convert.FromBase64String(serverDictionary["iv"]);
        byte[] encryptedVault = Convert.FromBase64String(serverDictionary["vault"]);

        Vault vault = DecryptVault(encryptedVault, vaultKey, vaultIv);
        return vault.Get(property);
    }

    private Vault DecryptVault(byte[] vault, byte[] vaultKey, byte[] vaultIv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = vaultKey;
            aes.IV = vaultIv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream msDecrypt = new MemoryStream(vault))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        string json = srDecrypt.ReadToEnd();
                        return JsonSerializer.Deserialize<Vault>(json)!;
                    }
                }
            }
        }
    }

    private byte[] EncryptVault(Vault vault, byte[] vaultKey, byte[] vaultIv)
    {
        // Serialize the vault to JSON
        string vaultJson = JsonSerializer.Serialize(vault);

        //  Encrypt the vault
        using (Aes aes = Aes.Create())
        {
            aes.Key = vaultKey;
            aes.IV = vaultIv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(vaultJson);
                    }
                }
                return msEncrypt.ToArray();
            }
        }
    }

    private byte[] GenerateVaultKey(string masterPassword, string secretKey)
    {
        byte[] masterPasswordBytes = Encoding.UTF8.GetBytes(masterPassword);
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

        int iterations = 10000; // Antal iterationer (kan justeras efter behov)
        int keyLength = 32; // Längden på valvnyckeln i byte (kan justeras efter behov)
        using Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(masterPasswordBytes, secretKeyBytes, iterations, HashAlgorithmName.SHA256);
        return deriveBytes.GetBytes(keyLength);
    }
}
