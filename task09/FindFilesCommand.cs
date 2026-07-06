using System;
using System.IO;
using task09;

namespace task09
{
    [DisplayName("Command for finding files by mask")]
    public class FindFilesCommand : ICommand
    {
        private readonly string _directoryPath;
        private readonly string _mask;

        public FindFilesCommand(string directoryPath, string mask)
        {
            _directoryPath = directoryPath;
            _mask = mask;
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
                return;

            var files = Directory.GetFiles(_directoryPath, _mask, SearchOption.AllDirectories);
        }
    }
}