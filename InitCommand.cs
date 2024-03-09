public class InitCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: init <client> <server>");
            return;
        }

        string clientFile = args[0];
        string serverFile = args[1];

        string masterPassword = ReadPasswordWithMinLength(8);

        Client client = new Client(clientFile);
        string secret = client.Init();

        Server server = new Server(serverFile);
        server.Init(masterPassword, secret);

        Console.WriteLine($"secret: {secret}");
    }

    private string ReadPasswordWithMinLength(int minLength)
    {
        string masterPassword;
        do
        {
            Console.Write($"Create master password (min length {minLength}): ");
            masterPassword = Console.ReadLine()!;
        } while (masterPassword.Length < minLength);
        return masterPassword;
    }
}
