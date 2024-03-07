public class SetCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: set <client> <server> <prop> {<value>}");
            return;
        }

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;

        Console.Write($"Enter value for {args[2]}: ");
        string value = Console.ReadLine()!;

        Client client = new Client(args[0]);
        string secretKey = client.GetSecretKey();
    
        Server server = new Server(args[1]);
        server.Set(masterPassword, secretKey, args[2], value);

        Console.WriteLine($"Key {args[2]} set successfully.");
    }
}
