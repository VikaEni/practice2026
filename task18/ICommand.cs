namespace task18
{
    public interface ICommand
    {
        bool IsCompleted { get; }
        void Execute();
    }
}