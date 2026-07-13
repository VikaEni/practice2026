using Xunit;
using task13;
using System;
using System.Collections.Generic;
using System.IO;

namespace task13tests
{
    public class StudentJsonTests
    {
        private Student CreateTestStudent()
        {
            return new Student
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(2000, 1, 15),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 5 },
                    new Subject { Name = "Physics", Grade = 4 },
                    new Subject { Name = "Programming", Grade = 5 }
                }
            };
        }

        [Fact]
        public void Serialize_ShouldReturnValidJson()
        {
            Student student = CreateTestStudent();
            string json = StudentJsonService.Serialize(student);

            Assert.Contains("firstName", json);
            Assert.Contains("John", json);
            Assert.Contains("lastName", json);
            Assert.Contains("Doe", json);
            Assert.Contains("birthDate", json);
            Assert.Contains("2000-01-15", json);
            Assert.Contains("grades", json);
        }

        [Fact]
        public void Deserialize_ShouldReturnValidStudent()
        {
            Student original = CreateTestStudent();
            string json = StudentJsonService.Serialize(original);

            Student result = StudentJsonService.Deserialize(json);

            Assert.NotNull(result);
            Assert.Equal(original.FirstName, result.FirstName);
            Assert.Equal(original.LastName, result.LastName);
            Assert.Equal(original.BirthDate, result.BirthDate);
            Assert.Equal(original.Grades.Count, result.Grades.Count);
        }

        [Fact]
        public void Deserialize_InvalidJson_ReturnsNull()
        {
            string invalidJson = "{ invalid }";
            Student result = StudentJsonService.Deserialize(invalidJson);
            Assert.Null(result);
        }

        [Fact]
        public void SaveAndLoadFromFile_ShouldWorkCorrectly()
        {
            Student student = CreateTestStudent();
            string filePath = Path.GetTempFileName();

            try
            {
                StudentJsonService.SaveToFile(student, filePath);
                Student loaded = StudentJsonService.LoadFromFile(filePath);

                Assert.NotNull(loaded);
                Assert.Equal(student.FirstName, loaded.FirstName);
                Assert.Equal(student.LastName, loaded.LastName);
                Assert.Equal(student.BirthDate, loaded.BirthDate);
                Assert.Equal(student.Grades.Count, loaded.Grades.Count);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Fact]
        public void Validate_ValidStudent_ReturnsTrue()
        {
            Student student = CreateTestStudent();
            bool result = StudentJsonService.Validate(student);
            Assert.True(result);
        }

        [Fact]
        public void Validate_InvalidName_ReturnsFalse()
        {
            Student student = CreateTestStudent();
            student.FirstName = "";
            bool result = StudentJsonService.Validate(student);
            Assert.False(result);
        }

        [Fact]
        public void Validate_InvalidBirthDate_ReturnsFalse()
        {
            Student student = CreateTestStudent();
            student.BirthDate = DateTime.Now.AddYears(1);
            bool result = StudentJsonService.Validate(student);
            Assert.False(result);
        }

        [Fact]
        public void Validate_InvalidGrade_ReturnsFalse()
        {
            Student student = CreateTestStudent();
            student.Grades[0].Grade = 10;
            bool result = StudentJsonService.Validate(student);
            Assert.False(result);
        }
    }
}