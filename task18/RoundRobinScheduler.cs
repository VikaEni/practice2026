using System;
using System.Collections.Generic;

namespace task18
{
    public class RoundRobinScheduler : IScheduler
    {
        private readonly List<ICommand> _commands = new List<ICommand>();
        private readonly object _lock = new object();
        private int _currentIndex = 0;

        public void Add(ICommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            lock (_lock)
            {
                _commands.Add(cmd);
            }
        }

        public bool HasCommand()
        {
            lock (_lock)
            {
                _commands.RemoveAll(c => c.IsCompleted);
                return _commands.Count > 0;
            }
        }

        public ICommand Select()
        {
            lock (_lock)
            {
                _commands.RemoveAll(c => c.IsCompleted);

                if (_commands.Count == 0)
                    return null;

                if (_currentIndex >= _commands.Count)
                    _currentIndex = 0;

                var command = _commands[_currentIndex];
                _currentIndex = (_currentIndex + 1) % _commands.Count;

                return command;
            }
        }
    }
}
