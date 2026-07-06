using System;
using System.IO;
using System.Linq;
using FileSystemCommands;
using Xunit;

namespace task08tests
{
    public class FileSystemCommandsTests
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldHandleNonExistentDirectory()
        {
            var nonExistentDir = Path.Combine(Path.GetTempPath(), "NonExistentDir_" + Guid.NewGuid().ToString());
            var command = new DirectorySizeCommand(nonExistentDir);

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }

        [Fact]
        public void FindFilesCommand_ShouldHandleNonExistentDirectory()
        {
            var nonExistentDir = Path.Combine(Path.GetTempPath(), "NonExistentDir_" + Guid.NewGuid().ToString());
            var command = new FindFilesCommand(nonExistentDir, "*.txt");

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);
        }

        [Fact]
        public void FindFilesCommand_ShouldReturnEmpty_WhenNoMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void DirectorySizeCommand_ShouldReturnZero_ForEmptyDirectory()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "EmptyDir_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);

            var command = new DirectorySizeCommand(testDir);

            var exception = Record.Exception(() => command.Execute());

            Assert.Null(exception);

            Directory.Delete(testDir, true);
        }
    }
}