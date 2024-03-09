public class CommandLine
{
    private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

    public CommandLine()
    {
        // Register all commands
        commands.Add("init", new InitCommand());
        commands.Add("set", new SetCommand());
        commands.Add("get", new GetCommand());
        commands.Add("delete", new DeleteCommand());
        commands.Add("secret", new SecretCommand());
        commands.Add("create", new CreateCommand());
    }

    public void ParseAndExecute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("usage: {init|create|get|set|delete|secret} <args>");
            return;
        }

        string commandName = args[0];
        if (commands.ContainsKey(commandName))
        {
            try
            {
                RunCommand(commands[commandName], args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{commandName} command failed ({ex.Message})");
            }
        }
        else
        {
            Console.WriteLine($"Unknown command: {commandName}");
        }
    }

    private void RunCommand(ICommand command, string[] args)
    {
        string[] commandArgs = args.Skip(1).ToArray();
        command.Execute(commandArgs);
    }
}