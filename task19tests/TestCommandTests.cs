using Xunit;
using task19;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace task19tests
{
    public class TestCommandTests
    {
        [Fact]
        public void TestCommand_ShouldExecuteMultipleTimes()
        {
            var cmd = new TestCommand(1, 3);

            int calls = 0;
            while (!cmd.IsCompleted)
            {
                cmd.Execute();
                calls++;
            }

            Assert.Equal(3, calls);
            Assert.Equal(3, cmd.GetCounter());
        }

        [Fact]
        public void TestCommand_ShouldStopAfterMaxCalls()
        {
            var cmd = new TestCommand(1, 5);

            for (int i = 0; i < 10; i++)
            {
                cmd.Execute();
            }

            Assert.True(cmd.IsCompleted);
            Assert.Equal(5, cmd.GetCounter());
        }

        [Fact]
        public void MultipleCommands_ShouldExecuteFairly()
        {
            var commands = new List<TestCommand>();

            for (int i = 0; i < 5; i++)
            {
                commands.Add(new TestCommand(i + 1, 3));
            }

            bool allCompleted = false;
            int round = 0;

            while (!allCompleted)
            {
                allCompleted = true;
                round++;

                foreach (var cmd in commands)
                {
                    if (!cmd.IsCompleted)
                    {
                        cmd.Execute();
                        allCompleted = false;
                    }
                }

                Assert.True(round <= 5);
            }

            foreach (var cmd in commands)
            {
                Assert.Equal(3, cmd.GetCounter());
            }
        }

        [Fact]
        public void GenerateReport()
        {
            var commands = new List<TestCommand>();
            var results = new Dictionary<int, List<int>>();

            for (int i = 0; i < 5; i++)
            {
                var cmd = new TestCommand(i + 1, 3);
                commands.Add(cmd);
                results[cmd.GetCounter()] = new List<int>();
            }

            bool allCompleted = false;
            int round = 0;

            while (!allCompleted)
            {
                allCompleted = true;
                round++;

                foreach (var cmd in commands)
                {
                    if (!cmd.IsCompleted)
                    {
                        cmd.Execute();
                        allCompleted = false;
                    }
                }
            }

            string report = "ОТЧЕТ\n";
            report += "5 экземпляров TestCommand, 3 вызова каждый\n\n";
            report += "Итоговый прогресс:\n";

            foreach (var cmd in commands)
            {
                report += $"Поток {cmd.GetCounter()}: {cmd.GetCounter()} вызовов\n";
            }

            report += $"\nВсего вызовов: {round * 5}\n";

            File.WriteAllText("report19.txt", report);
        }
    }
}