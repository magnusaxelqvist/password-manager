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

        Console.Write("Enter master password: ");
        string masterPassword = Console.ReadLine()!;
        
        Client client = new Client(clientFile);
        string secretKey = client.Init();
  
        Server server = new Server(serverFile);
        server.Init(masterPassword, secretKey);
        
        Console.WriteLine($"Client Secret: {secretKey}");
    }
}
