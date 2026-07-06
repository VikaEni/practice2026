using Xunit;
using System.Collections.Generic;
using System.Linq;
using task02;

public class StudentServiceTests
{
    private List<Student> _students;
    private StudentService _service;

    public StudentServiceTests()
    {
        _students = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } },
            new() { Name = "Виктория", Faculty = "Экономика", Grades = new List<int> { 3, 3, 3 } },
            new() { Name = "Данила", Faculty = "ФИТ", Grades = new List<int> { 5, 5, 4 } }
        };
        _service = new StudentService(_students);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(3, result.Count);
        Assert.All(result, s => Assert.Equal("ФИТ", s.Faculty));
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4.5).ToList();
        Assert.Equal(3, result.Count);
        Assert.Contains(result, s => s.Name == "Иван");
        Assert.Contains(result, s => s.Name == "Петр");
        Assert.Contains(result, s => s.Name == "Данила");
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsSortedStudents()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        Assert.Equal("Анна", result[0].Name);
        Assert.Equal("Виктория", result[1].Name);
        Assert.Equal("Данила", result[2].Name);
        Assert.Equal("Иван", result[3].Name);
        Assert.Equal("Петр", result[4].Name);
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectGroups()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.Equal(2, result.Count);
        Assert.Equal(3, result["ФИТ"].Count());
        Assert.Equal(2, result["Экономика"].Count());
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("ФИТ", result);
    }
}