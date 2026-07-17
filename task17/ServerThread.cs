using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public class ServerThread
    {
        private readonly ConcurrentQueue<ICommand> _commands = new ConcurrentQueue<ICommand>();
        private readonly ManualResetEventSlim _waitHandle = new ManualResetEventSlim(false);
        private readonly Thread _thread;
        private volatile bool _isRunning = true;
        private volatile bool _softStopRequested = false;

        public ServerThread()
        {
            _thread = new Thread(ProcessCommands)
            {
                IsBackground = true,
                Name = $"ServerThread-{Environment.CurrentManagedThreadId}"
            };
            _thread.Start();
        }

        public void AddCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _commands.Enqueue(command);
            _waitHandle.Set();
        }

        public void HardStop()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("HardStop must be called from the target thread");

            _isRunning = false;
            _softStopRequested = false;
            _waitHandle.Set();
        }

        public void SoftStop()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("SoftStop must be called from the target thread");

            _softStopRequested = true;
            _waitHandle.Set();
        }

        public void Join()
        {
            _thread.Join();
        }

        private void ProcessCommands()
        {
            while (_isRunning)
            {
                if (_softStopRequested && _commands.IsEmpty)
                    break;

                if (_commands.TryDequeue(out ICommand command))
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        OnException(command, ex);
                    }
                }
                else
                {
                    _waitHandle.Reset();
                    _waitHandle.Wait(100);
                }
            }
        }

        private void OnException(ICommand command, Exception ex)
        {
            Console.WriteLine($"Exception in command {command.GetType().Name}: {ex.Message}");
        }
    }
}