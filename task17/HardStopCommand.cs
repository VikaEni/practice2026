using System;

namespace task17
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _target;

        public HardStopCommand(ServerThread target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public void Execute()
        {
            _target.HardStop();
        }
    }
}