using System;
using task10;

namespace task10.Plugins
{
    [PluginLoad("PluginA")]
    public class PluginB : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("PluginB");
        }
    }
}