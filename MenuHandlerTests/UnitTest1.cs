using MasterOfPasswords.App;
using MasterOfPasswords.Commands;
using Moq;

namespace MenuHandlerTests;

public class MenuHandlerTests
{
    private readonly Mock<IPasswordManagerApp> _mockPasswordManagerApp;
    private readonly Mock<IConsoleManager> _mockConsoleManager;
    private readonly MenuHandler _menuHandler;

    public MenuHandlerTests()
    {
        _mockPasswordManagerApp = new Mock<IPasswordManagerApp>();
        _mockConsoleManager = new Mock<IConsoleManager>();
        _menuHandler = new MenuHandler(_mockPasswordManagerApp.Object, _mockConsoleManager.Object);
    }

    [Fact]
    public async Task DisplayMenuAsync_ShouldCallAddPassword_WhenOptionIs1()
    {
        _mockConsoleManager.SetupSequence(cm => cm.ReadInput())
            .Returns("1")
            .Returns("4"); // Завершение цикла

        await _menuHandler.DisplayMenuAsync();

        _mockPasswordManagerApp.Verify(app => app.AddPasswordAsync(), Times.Once);
    }

    [Fact]
    public async Task DisplayMenuAsync_ShouldCallFindPassword_WhenOptionIs2()
    {
        _mockConsoleManager.SetupSequence(cm => cm.ReadInput())
            .Returns("2")
            .Returns("4"); // Завершение цикла

        await _menuHandler.DisplayMenuAsync();

        _mockPasswordManagerApp.Verify(app => app.FindPasswordAsync(), Times.Once);
    }

    [Fact]
    public async Task DisplayMenuAsync_ShouldCallUpdatePassword_WhenOptionIs3()
    {
        _mockConsoleManager.SetupSequence(cm => cm.ReadInput())
            .Returns("3")
            .Returns("4"); // Завершение цикла

        await _menuHandler.DisplayMenuAsync();

        _mockPasswordManagerApp.Verify(app => app.UpdatePasswordAsync(), Times.Once);
    }

    [Fact]
    public async Task DisplayMenuAsync_ShouldExit_WhenOptionIs4()
    {
        _mockConsoleManager.SetupSequence(cm => cm.ReadInput())
            .Returns("4"); // Завершение цикла

        await _menuHandler.DisplayMenuAsync();

        _mockPasswordManagerApp.Verify(app => app.AddPasswordAsync(), Times.Never);
        _mockPasswordManagerApp.Verify(app => app.FindPasswordAsync(), Times.Never);
        _mockPasswordManagerApp.Verify(app => app.UpdatePasswordAsync(), Times.Never);
    }

    [Fact]
    public async Task DisplayMenuAsync_ShouldDisplayError_WhenOptionIsInvalid()
    {
        _mockConsoleManager.SetupSequence(cm => cm.ReadInput())
            .Returns("invalid")
            .Returns("4"); // Завершение цикла

        await _menuHandler.DisplayMenuAsync();

        _mockConsoleManager.Verify(cm => cm.WriteError("Неверный формат ввода."), Times.Once);
    }

    [Fact]
    public async Task DisplayMenuAsync_ShouldDisplayGoodbyeMessage_WhenExiting()
    {
        _mockConsoleManager.SetupSequence(cm => cm.ReadInput())
            .Returns("4"); // Завершение цикла

        await _menuHandler.DisplayMenuAsync();

        _mockConsoleManager.Verify(cm => cm.WriteInformation("До свидания!"), Times.Once);
    }
}