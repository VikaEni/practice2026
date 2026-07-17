namespace task19
{
    public interface ICommand
    {
        bool IsCompleted { get; }
        void Execute();
    }
}