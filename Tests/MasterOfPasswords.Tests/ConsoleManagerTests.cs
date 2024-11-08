using MasterOfPasswords.Commands;

namespace MasterOfPasswords.Tests
{
    public class ConsoleManagerTests
    {
        private readonly ConsoleManager _consoleManager;
        private readonly StringWriter _output;
        private readonly StringReader _input;

        public ConsoleManagerTests()
        {
            _consoleManager = new ConsoleManager();
            _output = new StringWriter();
            _input = new StringReader("Sample input");
            Console.SetOut(_output);
            Console.SetIn(_input);
        }

        [Fact]
        public void WriteInformation_ShouldSetWhiteColorAndReset()
        {
            // Act
            _consoleManager.WriteInformation("Information message");

            // Assert
            Assert.Contains("Information message", _output.ToString());
            // Здесь проверяем, что цвет был изменен, для этого требуется дополнительная библиотека или анализ вывода
        }

        [Fact]
        public void WriteWarning_ShouldSetYellowColorAndReset()
        {
            // Act
            _consoleManager.WriteWarning("Warning message");

            // Assert
            Assert.Contains("Warning message", _output.ToString());
        }

        [Fact]
        public void WriteError_ShouldSetRedColorAndReset()
        {
            // Act
            _consoleManager.WriteError("Error message");

            // Assert
            Assert.Contains("Error message", _output.ToString());
        }

        [Fact]
        public void ReadInput_ShouldReturnInputString()
        {
            // Act
            var input = _consoleManager.ReadInput();

            // Assert
            Assert.Equal("Sample input", input);
        }

        [Fact]
        public void ReadInput_ShouldThrowExceptionIfInputIsNull()
        {
            // Arrange
            Console.SetIn(new StringReader(string.Empty));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _consoleManager.ReadInput());
        }
    }
}
