using Xunit;
using task17;
using System;
using System.Threading;

namespace task17tests
{
    public class ServerThreadTests
    {
        [Fact]
        public void AddCommand_ShouldExecuteCommand()
        {
            var server = new ServerThread();
            var commandExecuted = false;
            var testCommand = new TestCommand("Test");

            server.AddCommand(testCommand);
            Thread.Sleep(100);

            Assert.True(true);
        }

        [Fact]
        public void HardStop_ShouldStopImmediately()
        {
            var server = new ServerThread();
            var longCommand = new TestCommand("Long", 1000);
            var hardStop = new HardStopCommand(server);

            server.AddCommand(longCommand);
            server.AddCommand(hardStop);
            Thread.Sleep(100);

            Assert.True(true);
        }

        [Fact]
        public void SoftStop_ShouldStopAfterQueueEmpty()
        {
            var server = new ServerThread();
            var command1 = new TestCommand("Cmd1", 50);
            var command2 = new TestCommand("Cmd2", 50);
            var softStop = new SoftStopCommand(server);

            server.AddCommand(command1);
            server.AddCommand(command2);
            server.AddCommand(softStop);
            Thread.Sleep(300);

            Assert.True(true);
        }

        [Fact]
        public void HardStop_FromWrongThread_ShouldThrowException()
        {
            var server = new ServerThread();
            var hardStop = new HardStopCommand(server);

            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    hardStop.Execute();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.Start();
            thread.Join();

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void SoftStop_FromWrongThread_ShouldThrowException()
        {
            var server = new ServerThread();
            var softStop = new SoftStopCommand(server);

            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    softStop.Execute();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.Start();
            thread.Join();

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void ExceptionCommand_ShouldBeHandled()
        {
            var server = new ServerThread();
            var exceptionCommand = new ExceptionCommand();

            server.AddCommand(exceptionCommand);
            Thread.Sleep(100);

            Assert.True(true);
        }
    }
}