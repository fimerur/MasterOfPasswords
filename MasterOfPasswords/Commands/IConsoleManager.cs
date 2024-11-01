namespace MasterOfPasswords.Commands;

public interface IConsoleManager
{
    void WriteInformation(string message);
    void WriteWarning(string message);
    void WriteError(string message);
    string ReadInput();
}