using Avalonia;
using Avalonia.ReactiveUI;

namespace MasterOfPasswords.UI;

class Program
{
    public static void Main(string[] args)
    {
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI()
            .StartWithClassicDesktopLifetime(args);
    }
}