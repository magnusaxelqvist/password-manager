public class SecretKey
{
    private static readonly char[] validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!#%".ToCharArray();

    public static string GenerateRandomString(int length)
    {
        var random = new Random();
        var randomString = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomString[i] = validCharacters[random.Next(validCharacters.Length)];
        }
        return new string(randomString);
    }
}
