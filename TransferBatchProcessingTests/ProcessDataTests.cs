using FluentAssertions;
using TransferBatchProcessing;

namespace TransferBatchProcessingTests
{
    public class ProcessDataTests
    {

        [Fact]
        public void ReadFileAndCalculateCommissions_NoArgs_ReturnValidationError()
        {
            // Arrange
            string[] args = [];

            // Act
            var output = CaptureConsoleOutput(() =>
            {
                ProcessData.ReadFileAndCalculateCommissions(args);
            });

            // Assert
            output.Should().Contain($"Please, give a file to process");
        }

        [Fact]
        public void ReadFileAndCalculateCommissions_ArgsEmpty_ReturnValidationError()
        {
            // Arrange
            string[] args = [string.Empty];

            // Act
            var act = () => ProcessData.ReadFileAndCalculateCommissions(args);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("The path is empty. (Parameter 'path')");
        }

        [Fact]
        public void ReadFileAndCalculateCommissions_InvalidFilePath_ReturnValidationError()
        {
            // Arrange
            var filePath = "invalidPath.csv";
            var args = new[] { filePath };
            var invalidFilePath = Path.GetFullPath(args[0]);

            // Act
            var output = CaptureConsoleOutput(() =>
            {
                ProcessData.ReadFileAndCalculateCommissions(args);
            });

            // Assert
            output.Should().Contain($"File {invalidFilePath} does not exist.");
        }

        [Fact]
        public void ReadFileAndCalculateCommissions_ValidFile_CalculatesCommissionsCorrectly()
        {
            // Arrange
            var filePath = "testTransfers.csv";
            var testData = new[]
            {
                "A10,T1000,100.00",
                "A10,T1001,200.00",
                "A10,T1002,300.00",
                "A11,T1003,400.00"
            };

            File.WriteAllLines(filePath, testData);
            var args = new[] { filePath };

            // Act
            var output = CaptureConsoleOutput(() =>
            {
                ProcessData.ReadFileAndCalculateCommissions(args);
            });

            // Assert
            output.Should().Contain("A10,30,00");
            output.Should().Contain("A11,40,00");
            RemoveFileIfExists(filePath);
        }

        [Fact]
        public void ReadFileAndCalculateCommissions_FileWithSomeInvalidData_CalculatesCommissionsCorrectly()
        {
            // Arrange
            var filePath = "testTransfers.csv";
            var testData = new[]
            {
                "A10,T1000",
                "A10,T1001,200.00",
                "A10,T1002,300.00",
                "A11,T1003,zzz"
            };

            File.WriteAllLines(filePath, testData);
            var args = new[] { filePath };

            // Act
            var output = CaptureConsoleOutput(() =>
            {
                ProcessData.ReadFileAndCalculateCommissions(args);
            });

            // Assert
            output.Should().Contain("A10,20,00");
            RemoveFileIfExists(filePath);
        }

        private static void RemoveFileIfExists(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private static string CaptureConsoleOutput(Action action)
        {
            var originalConsoleOut = Console.Out;
            using var sw = new StringWriter();
            try
            {
                Console.SetOut(sw);
                action();
                return sw.ToString();
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }
        }
    }
}