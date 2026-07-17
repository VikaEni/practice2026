using System;
using System.Threading;

namespace task18
{
    public class LongRunningCommand : ICommand
    {
        private readonly string _name;
        private readonly int _totalSteps;
        private int _currentStep = 0;
        private readonly int _stepsPerExecute = 10;

        public bool IsCompleted => _currentStep >= _totalSteps;

        public LongRunningCommand(string name, int totalSteps)
        {
            _name = name;
            _totalSteps = totalSteps;
        }

        public void Execute()
        {
            if (IsCompleted)
                return;

            int stepsToDo = Math.Min(_stepsPerExecute, _totalSteps - _currentStep);

            for (int i = 0; i < stepsToDo; i++)
            {
                _currentStep++;
                Thread.Sleep(1);
            }

            Console.WriteLine($"[{_name}] Progress: {_currentStep}/{_totalSteps}");
        }

        public int GetProgress() => _currentStep;
        public int GetTotalSteps() => _totalSteps;
        public string GetName() => _name;
    }
}