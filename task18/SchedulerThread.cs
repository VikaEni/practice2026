using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task18
{
    public class SchedulerThread
    {
        private readonly ConcurrentQueue<ICommand> _newCommands = new ConcurrentQueue<ICommand>();
        private readonly IScheduler _scheduler;
        private readonly Thread _thread;
        private volatile bool _isRunning = true;
        private readonly ManualResetEventSlim _waitHandle = new ManualResetEventSlim(false);

        public SchedulerThread(IScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _thread = new Thread(ProcessCommands)
            {
                IsBackground = true,
                Name = $"SchedulerThread-{Environment.CurrentManagedThreadId}"
            };
            _thread.Start();
        }

        public void AddCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _newCommands.Enqueue(command);
            _waitHandle.Set();
        }

        public void Stop()
        {
            _isRunning = false;
            _waitHandle.Set();
            _thread.Join();
        }

        private void ProcessCommands()
        {
            while (_isRunning)
            {
                while (_newCommands.TryDequeue(out ICommand newCmd))
                {
                    _scheduler.Add(newCmd);
                }

                if (_scheduler.HasCommand())
                {
                    ICommand command = _scheduler.Select();
                    if (command != null && !command.IsCompleted)
                    {
                        try
                        {
                            command.Execute();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception: {ex.Message}");
                        }
                    }
                }
                else
                {
                    _waitHandle.Reset();
                    _waitHandle.Wait(50);
                }
            }
        }
    }
}