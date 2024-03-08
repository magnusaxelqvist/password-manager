public class SetCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: set <client> <server> <prop> {<value>}");
            return;
        }

        string clientFile = args[0];
        string serverFile = args[1];
        string property = args[2];

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;

        Console.Write($"Enter value for {args[2]}: ");
        string value = Console.ReadLine()!;

        Client client = new Client(clientFile);
        string secretKey = client.GetSecretKey();

        Server server = new Server(serverFile);
        server.SetProperty(masterPassword, secretKey, property, value);

        Console.WriteLine($"Key {args[2]} set successfully.");
    }
}
