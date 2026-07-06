using System;
using System.IO;
using System.Reflection;
using System.Linq;

namespace MetadataViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Specify path to DLL");
                return;
            }

            string path = args[0];

            if (!File.Exists(path))
            {
                Console.WriteLine("File not found");
                return;
            }

            Assembly asm = Assembly.LoadFrom(path);
            Console.WriteLine("Assembly: " + asm.GetName().Name);
            Console.WriteLine();

            var classes = asm.GetTypes().Where(t => t.IsClass && !t.IsAbstract);

            foreach (var cls in classes)
            {
                Console.WriteLine("Class: " + cls.Name);

                var attrs = cls.GetCustomAttributes();
                if (attrs.Any())
                {
                    Console.WriteLine("  Attributes:");
                    foreach (var a in attrs)
                        Console.WriteLine("    - " + a.GetType().Name);
                }

                var ctors = cls.GetConstructors();
                if (ctors.Any())
                {
                    Console.WriteLine("  Constructors:");
                    foreach (var c in ctors)
                    {
                        var pars = c.GetParameters();
                        string s = string.Join(", ", pars.Select(p => p.ParameterType.Name + " " + p.Name));
                        Console.WriteLine("    - " + cls.Name + "(" + s + ")");
                    }
                }

                var methods = cls.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (methods.Any())
                {
                    Console.WriteLine("  Methods:");
                    foreach (var m in methods)
                    {
                        var pars = m.GetParameters();
                        string s = string.Join(", ", pars.Select(p => p.ParameterType.Name + " " + p.Name));
                        Console.WriteLine("    - " + m.ReturnType.Name + " " + m.Name + "(" + s + ")");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Total: " + classes.Count());
        }
    }
}