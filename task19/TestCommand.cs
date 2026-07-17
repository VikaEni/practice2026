using System;

namespace task19
{
    public class TestCommand : ICommand
    {
        private readonly int _id;
        private int _counter = 0;
        private readonly int _maxCalls;

        public bool IsCompleted => _counter >= _maxCalls;

        public TestCommand(int id, int maxCalls = 3)
        {
            _id = id;
            _maxCalls = maxCalls;
        }

        public void Execute()
        {
            if (IsCompleted)
                return;

            _counter++;
            Console.WriteLine($"Поток {_id} вызов {_counter}");
        }

        public int GetCounter() => _counter;
    }
}