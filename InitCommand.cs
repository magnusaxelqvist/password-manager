public class InitCommand : ICommand
{
    public void Execute(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: init <client> <server>");
            return;
        }

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;
  
        Client client = new Client(args[0]);
        string secretKey = client.Init();

        Server server = new Server(args[1]);
        server.Init(masterPassword, secretKey);
        
        Console.WriteLine($"Client Secret: {secretKey}");
    }
}
