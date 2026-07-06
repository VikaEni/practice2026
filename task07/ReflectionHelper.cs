using System;
using System.Reflection;
using System.Text;

namespace task07
{
    public static class ReflectionHelper
    {
        public static string PrintTypeInfo(Type type)
        {
            var sb = new StringBuilder();

            sb.AppendLine(type.Name);

            var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
            sb.AppendLine(displayName?.DisplayName ?? "");

            var version = type.GetCustomAttribute<VersionAttribute>();
            sb.AppendLine(version != null ? $"{version.Major}.{version.Minor}" : "");

            foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var attr = m.GetCustomAttribute<DisplayNameAttribute>();
                sb.AppendLine($"{m.Name} {attr?.DisplayName ?? ""}");
            }

            foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = p.GetCustomAttribute<DisplayNameAttribute>();
                sb.AppendLine($"{p.Name} {attr?.DisplayName ?? ""}");
            }

            return sb.ToString();
        }
    }
}