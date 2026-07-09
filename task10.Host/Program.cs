using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using task10;

namespace task10.Host
{
    class Program
    {
        static void Main()
        {
            string pluginsPath = Path.Combine(Directory.GetCurrentDirectory(), "task10.Plugins", "bin", "Debug", "net8.0");
            if (!Directory.Exists(pluginsPath))
                return;

            var dllFiles = Directory.GetFiles(pluginsPath, "*.dll");
            var pluginTypes = new List<Type>();
            var dependencyGraph = new Dictionary<string, List<string>>();

            foreach (var dll in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);
                    var types = assembly.GetTypes()
                        .Where(t => t.IsClass && t.GetCustomAttribute<PluginLoadAttribute>() != null);

                    foreach (var type in types)
                    {
                        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                        pluginTypes.Add(type);
                        dependencyGraph[type.Name] = attr.Dependencies.ToList();
                    }
                }
                catch { }
            }

            if (pluginTypes.Count == 0)
                return;

            var sortedPlugins = TopologicalSort(pluginTypes, dependencyGraph);

            foreach (var type in sortedPlugins)
            {
                try
                {
                    var plugin = Activator.CreateInstance(type) as IPlugin;
                    plugin?.Execute();
                }
                catch { }
            }
        }

        static List<Type> TopologicalSort(List<Type> types, Dictionary<string, List<string>> graph)
        {
            var result = new List<Type>();
            var visited = new HashSet<string>();
            var visiting = new HashSet<string>();

            foreach (var type in types)
                if (!visited.Contains(type.Name))
                    Visit(type.Name, types, graph, visited, visiting, result);

            return result;
        }

        static void Visit(string name, List<Type> types, Dictionary<string, List<string>> graph,
                         HashSet<string> visited, HashSet<string> visiting, List<Type> result)
        {
            if (visiting.Contains(name))
                throw new Exception($"Cycle: {name}");

            if (!visited.Contains(name))
            {
                visiting.Add(name);

                if (graph.TryGetValue(name, out var deps))
                    foreach (var dep in deps)
                        Visit(dep, types, graph, visited, visiting, result);

                visiting.Remove(name);
                visited.Add(name);

                var type = types.FirstOrDefault(t => t.Name == name);
                if (type != null)
                    result.Add(type);
            }
        }
    }
}