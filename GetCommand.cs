public class GetCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: get <client> <server> [<prop >] {<pwd >}");
            return;
        }

        string clientFile = args[0];
        string serverFile = args[1];
        string property = args[2];

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;

        Client client = new Client(clientFile);
        string secretKey = client.GetSecretKey();

        Server server = new Server(serverFile); 
        string? value = server.Get(masterPassword, secretKey, property);

        if (value != null)
        {
            Console.WriteLine($"Value: {value}");
        }
        else
        {
            Console.WriteLine("Property not found.");
        }
    }
}
