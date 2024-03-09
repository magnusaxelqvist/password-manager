public class DeleteCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: delete <client> <server> <prop> {<pwd>}");
            return;
        }

        string clientFile = args[0];
        string serverFile = args[1];
        string property = args[2];

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;

        Client client = new Client(clientFile);
        string secret = client.GetSecret();

        Server server = new Server(serverFile);
        string? value = server.GetProperty(masterPassword, secret, property);
        if (value != null)
        {
            server.DeleteProperty(masterPassword, secret, property);
            Console.WriteLine($"{property} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"{property} not found.");
        }
    }
}
