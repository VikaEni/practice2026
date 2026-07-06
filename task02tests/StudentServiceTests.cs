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
            new() { Name = "Ivan", Faculty = "FIT", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Anna", Faculty = "FIT", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Petr", Faculty = "Economics", Grades = new List<int> { 5, 5, 5 } },
            new() { Name = "Victoria", Faculty = "Economics", Grades = new List<int> { 3, 3, 3 } },
            new() { Name = "Danila", Faculty = "FIT", Grades = new List<int> { 5, 5, 4 } }
        };
        _service = new StudentService(_students);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("FIT").ToList();
        Assert.Equal(3, result.Count);
        Assert.All(result, s => Assert.Equal("FIT", s.Faculty));
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4.5).ToList();
        Assert.Equal(3, result.Count);
        Assert.Contains(result, s => s.Name == "Ivan");
        Assert.Contains(result, s => s.Name == "Petr");
        Assert.Contains(result, s => s.Name == "Danila");
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsSortedStudents()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        Assert.Equal("Anna", result[0].Name);
        Assert.Equal("Danila", result[1].Name);
        Assert.Equal("Ivan", result[2].Name);
        Assert.Equal("Petr", result[3].Name);
        Assert.Equal("Victoria", result[4].Name);
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectGroups()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.Equal(2, result.Count);
        Assert.Equal(3, result["FIT"].Count());
        Assert.Equal(2, result["Economics"].Count());
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("FIT", result);
    }
}