using System;

namespace task17
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _target;

        public SoftStopCommand(ServerThread target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public void Execute()
        {
            _target.SoftStop();
        }
    }
}