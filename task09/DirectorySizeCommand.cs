using System;
using System.IO;
using System.Linq;
using task09;

namespace task09
{
    [DisplayName("Command for calculating folder size")]
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _directoryPath;

        public DirectorySizeCommand(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
                return;

            long size = 0;
            var files = Directory.GetFiles(_directoryPath, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                size += new FileInfo(file).Length;
            }
        }
    }
}