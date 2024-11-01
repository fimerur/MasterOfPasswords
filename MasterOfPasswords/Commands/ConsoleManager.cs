namespace MasterOfPasswords.Commands;

public class ConsoleManager : IConsoleManager
{
    public void WriteInformation(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public string ReadInput()
    {
        return Console.ReadLine() ?? throw new InvalidOperationException();
    }
}