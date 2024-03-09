public class GetCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: get <client> <server> [<prop>] {<pwd>}");
            return;
        }

        string clientFile = args[0];
        string serverFile = args[1];
        string? property = args.Length > 2 ? args[2] : null;

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;

        Client client = new Client(clientFile);
        string secret = client.GetSecret();

        Server server = new Server(serverFile);
        if (property != null)
        {
            string? value = server.GetProperty(masterPassword, secret, property);
            if (value != null)
            {
                Console.WriteLine($"Value: {value}");
            }
            else
            {
                Console.WriteLine("Property not found.");
            }
        }
        else
        {
            foreach (var prop in server.GetAllProperties(masterPassword, secret))
            {
                Console.WriteLine($"{prop}");
            }
        }
    }
}
