public class Program
{
    public static void Main(string[] args)
    {
        CommandLine cmdLine = new CommandLine();
        cmdLine.ParseAndExecute(args);
    }
}
