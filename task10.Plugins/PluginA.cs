using System;
using task10;

namespace task10.Plugins
{
    [PluginLoad]
    public class PluginA : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("PluginA");
        }
    }
}