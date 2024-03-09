using System.Text.Json;

public class Server
{
    private string filePath;

    public Server(string filePath)
    {
        this.filePath = filePath;
    }

    public void Init(string masterPassword, string secret)
    {
        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secret);
        byte[] vaultIv = Crypto.GenerateIv();

        FileDictionary dictionary = new FileDictionary(filePath);
        dictionary.Set("iv", Convert.ToBase64String(vaultIv));
        dictionary.Set("vault", Convert.ToBase64String(Crypto.EncryptVault(new Vault(), vaultKey, vaultIv)));
        dictionary.Save();
    }

    public void Validate(string masterPassword, string secret) {
        FileDictionary dictionary = new FileDictionary(filePath).Load();
        GetVaultFromDictionary(dictionary, masterPassword, secret);
    }

    public string? GetProperty(string masterPassword, string secret, string property)
    {
        FileDictionary dictionary = new FileDictionary(filePath).Load();
        Vault vault = GetVaultFromDictionary(dictionary, masterPassword, secret);
        return vault.Get(property);
    }

    public void SetProperty(string masterPassword, string secret, string property, string value)
    {
        FileDictionary dictionary = new FileDictionary(filePath).Load();
        Vault vault = GetVaultFromDictionary(dictionary, masterPassword, secret);
        vault.Set(property, value);

        SetVaultInDictionary(dictionary, vault, masterPassword, secret);
        dictionary.Save();
    }

    private Vault GetVaultFromDictionary(FileDictionary dictionary, string masterPassword, string secret)
    {
        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secret);
        byte[] vaultIv = Convert.FromBase64String(dictionary.Get("iv")!);
        byte[] encryptedVault = Convert.FromBase64String(dictionary.Get("vault")!);

        return Crypto.DecryptVault(encryptedVault, vaultKey, vaultIv);
    }

    private void SetVaultInDictionary(FileDictionary dictionary, Vault vault, string masterPassword, string secret)
    {
        byte[] vaultKey = Crypto.GenerateVaultKey(masterPassword, secret);
        byte[] vaultIv = Convert.FromBase64String(dictionary.Get("iv")!);
        dictionary.Set("vault", Convert.ToBase64String(Crypto.EncryptVault(vault, vaultKey, vaultIv)));
    }

    public IEnumerable<string> GetAllProperties(string masterPassword, string secret)
    {
        FileDictionary dictionary = new FileDictionary(filePath).Load();
        Vault vault = GetVaultFromDictionary(dictionary, masterPassword, secret);
        return vault.GetAllProperties();
    }

    internal void DeleteProperty(string masterPassword, string secret, string property)
    {
        FileDictionary dictionary = new FileDictionary(filePath).Load();
        Vault vault = GetVaultFromDictionary(dictionary, masterPassword, secret);
        vault.Delete(property);

        SetVaultInDictionary(dictionary, vault, masterPassword, secret);
        dictionary.Save();
    }
}
