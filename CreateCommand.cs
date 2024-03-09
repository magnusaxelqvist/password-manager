public class CreateCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: init <client> <server> {<pwd>} {<secret>}");
            return;
        }

        string clientFile = args[0];
        string serverFile = args[1];

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;

        Console.Write("Enter secret: ");
        string secret = Console.ReadLine()!;

        new Server(serverFile).Validate(masterPassword, secret);
        new Client(clientFile).Init(secret);

        Console.WriteLine($"secret: {secret}");
    }
}
