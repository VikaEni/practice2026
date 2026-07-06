using System;
using System.IO;
using System.Reflection;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main()
        {
            string dllPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..",
                                         "FileSystemCommands", "bin", "Debug", "net8.0", "FileSystemCommands.dll");

            if (!File.Exists(dllPath))
                return;

            Assembly assembly = Assembly.LoadFrom(dllPath);

            Type dirSizeType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
            var dirSizeCommand = Activator.CreateInstance(dirSizeType, new object[] { Directory.GetCurrentDirectory() }) as ICommand;
            dirSizeCommand?.Execute();

            Type findFilesType = assembly.GetType("FileSystemCommands.FindFilesCommand");
            var findFilesCommand = Activator.CreateInstance(findFilesType, new object[] { Directory.GetCurrentDirectory(), "*.cs" }) as ICommand;
            findFilesCommand?.Execute();
        }
    }
}