public class SecretCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: secret <client>");
            return;
        }

        string clientFile = args[0];
        Client client = new Client(clientFile);
        string secretKey = client.GetSecretKey();
        Console.WriteLine($"Client Secret: {secretKey}");
    }
}
