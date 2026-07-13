using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public static class StudentJsonService
    {
        private static JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new CustomDateTimeConverter() }
            };
        }

        public static string Serialize(Student student)
        {
            JsonSerializerOptions options = GetOptions();
            return JsonSerializer.Serialize(student, options);
        }

        public static Student Deserialize(string json)
        {
            try
            {
                JsonSerializerOptions options = GetOptions();
                return JsonSerializer.Deserialize<Student>(json, options);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public static void SaveToFile(Student student, string filePath)
        {
            string json = Serialize(student);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        public static Student LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath, Encoding.UTF8);
            return Deserialize(json);
        }

        public static bool Validate(Student student)
        {
            if (student == null)
                return false;

            if (string.IsNullOrWhiteSpace(student.FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(student.LastName))
                return false;

            if (student.BirthDate > DateTime.Now)
                return false;

            if (student.BirthDate < DateTime.Now.AddYears(-100))
                return false;

            if (student.Grades == null)
                return false;

            foreach (Subject subject in student.Grades)
            {
                if (string.IsNullOrWhiteSpace(subject.Name))
                    return false;

                if (subject.Grade < 1 || subject.Grade > 5)
                    return false;
            }

            return true;
        }
    }
}