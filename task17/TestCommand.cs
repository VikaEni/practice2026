using System;
using System.Threading;

namespace task17
{
    public class TestCommand : ICommand
    {
        private readonly string _name;
        private readonly int _delayMs;

        public TestCommand(string name, int delayMs = 0)
        {
            _name = name;
            _delayMs = delayMs;
        }

        public void Execute()
        {
            Console.WriteLine($"Executing: {_name} on thread {Thread.CurrentThread.ManagedThreadId}");

            if (_delayMs > 0)
                Thread.Sleep(_delayMs);

            Console.WriteLine($"Completed: {_name}");
        }
    }

    public class ExceptionCommand : ICommand
    {
        public void Execute()
        {
            throw new InvalidOperationException("Test exception from command");
        }
    }
}