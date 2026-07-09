using System;
using task10;

namespace task10.Plugins
{
    [PluginLoad("PluginA", "PluginB")]
    public class PluginC : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("PluginC");
        }
    }
}