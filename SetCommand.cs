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
            value = Crypto.GenerateSecretKey(8);
            Console.WriteLine($"Generated password for {property}: {value}");
        }
        else
        {
            Console.Write($"Enter value for {property}: ");
            value = Console.ReadLine()!;
        }

        Client client = new Client(clientFile);
        string secretKey = client.GetSecretKey();

        Server server = new Server(serverFile);
        server.SetProperty(masterPassword, secretKey, property, value);

        Console.WriteLine($"Key {property} set successfully.");
    }
}
