using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class Crypto
{
    public static string GenerateSecret(int length)
    {
        byte[] secret = new byte[length];
        RandomNumberGenerator.Create().GetBytes(secret);
        return Convert.ToBase64String(secret);
    }

    public static Vault DecryptVault(byte[] vault, byte[] vaultKey, byte[] vaultIv)
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

    public static byte[] EncryptVault(Vault vault, byte[] vaultKey, byte[] vaultIv)
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

    public static byte[] GenerateVaultKey(string masterPassword, string secretKey)
    {
        byte[] masterPasswordBytes = Encoding.UTF8.GetBytes(masterPassword);
        byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

        int iterations = 10000;
        int keyLength = 32;
        using Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(masterPasswordBytes, secretKeyBytes, iterations, HashAlgorithmName.SHA256);
        return deriveBytes.GetBytes(keyLength);
    }

    internal static byte[] GenerateIv()
    {
        return Aes.Create().IV;
    }
}
