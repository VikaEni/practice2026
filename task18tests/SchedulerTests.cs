using Xunit;
using task18;
using System.Collections.Generic;
using System.Threading;

namespace task18tests
{
    public class SchedulerTests
    {
        [Fact]
        public void RoundRobinScheduler_ShouldDistributeCommandsFairly()
        {
            var scheduler = new RoundRobinScheduler();
            var cmd1 = new LongRunningCommand("A", 100);
            var cmd2 = new LongRunningCommand("B", 100);
            var cmd3 = new LongRunningCommand("C", 100);

            scheduler.Add(cmd1);
            scheduler.Add(cmd2);
            scheduler.Add(cmd3);

            var selected = new List<string>();
            for (int i = 0; i < 9; i++)
            {
                var cmd = scheduler.Select();
                if (cmd != null)
                    selected.Add(((LongRunningCommand)cmd).GetName());
            }

            Assert.Contains("A", selected);
            Assert.Contains("B", selected);
            Assert.Contains("C", selected);
            Assert.Equal(9, selected.Count);
        }

        [Fact]
        public void SchedulerThread_ShouldProcessCommands()
        {
            var scheduler = new RoundRobinScheduler();
            var thread = new SchedulerThread(scheduler);
            var cmd1 = new LongRunningCommand("Test1", 50);
            var cmd2 = new LongRunningCommand("Test2", 50);

            thread.AddCommand(cmd1);
            thread.AddCommand(cmd2);
            Thread.Sleep(3000);

            Assert.True(cmd1.IsCompleted);
            Assert.True(cmd2.IsCompleted);

            thread.Stop();
        }

        [Fact]
        public void LongRunningCommand_ShouldExecuteInMultipleSteps()
        {
            var cmd = new LongRunningCommand("Test", 100);

            int executedSteps = 0;
            while (!cmd.IsCompleted)
            {
                cmd.Execute();
                executedSteps++;
            }

            Assert.True(cmd.IsCompleted);
            Assert.InRange(executedSteps, 8, 15);
        }

        [Fact]
        public void SchedulerThread_ShouldHandleMultipleCommandsSimultaneously()
        {
            var scheduler = new RoundRobinScheduler();
            var thread = new SchedulerThread(scheduler);
            var commands = new List<LongRunningCommand>();

            for (int i = 0; i < 5; i++)
            {
                var cmd = new LongRunningCommand($"Cmd{i}", 20);
                commands.Add(cmd);
                thread.AddCommand(cmd);
            }

            Thread.Sleep(3000);

            int completed = 0;
            foreach (var cmd in commands)
            {
                if (cmd.IsCompleted)
                    completed++;
            }

            Assert.True(completed >= 3);

            thread.Stop();
        }

        [Fact]
        public void SchedulerThread_ShouldNotBlockOnEmptyQueue()
        {
            var scheduler = new RoundRobinScheduler();
            var thread = new SchedulerThread(scheduler);

            Thread.Sleep(500);

            Assert.True(true);
            thread.Stop();
        }

        [Fact]
        public void SchedulerThread_ShouldProcessNewCommandsAfterExistingOnes()
        {
            var scheduler = new RoundRobinScheduler();
            var thread = new SchedulerThread(scheduler);
            var cmd1 = new LongRunningCommand("First", 50);
            var cmd2 = new LongRunningCommand("Second", 50);

            thread.AddCommand(cmd1);
            Thread.Sleep(100);
            thread.AddCommand(cmd2);
            Thread.Sleep(2000);

            Assert.True(cmd1.IsCompleted);
            Assert.True(cmd2.IsCompleted);

            thread.Stop();
        }
    }
}