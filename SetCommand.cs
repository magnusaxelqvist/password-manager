
public class SetCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: set <client> <server> <prop> [-g] {<pwd>} {<value>}");
            return;
        }

        string clientFile = args[0];
        string serverFile = args[1];
        string property = args[2];
        bool generateValue = args.Contains("-g");

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;

        string value;
        if (generateValue)
        {
            value = Crypto.GenerateSecret(8);
            Console.WriteLine($"password for {property}: {value}");
        }
        else
        {
            Console.Write($"Enter value for {property}: ");
            value = Console.ReadLine()!;
        }

        Client client = new Client(clientFile);
        string secret = client.GetSecret();

        Server server = new Server(serverFile);
        server.SetProperty(masterPassword, secret, property, value);

        Console.WriteLine($"{property} set successfully.");
    }
}
