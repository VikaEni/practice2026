using System;
using System.Collections.Generic;
using System.Linq;

namespace task02
{
    public class StudentService
    {
        private readonly List<Student> _students;

        public StudentService(List<Student> students)
        {
            _students = students;
        }

        public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        {
            return _students.Where(s => s.Faculty == faculty);
        }

        public IEnumerable<Student> GetStudentsWithMinAverageGrade(double min)
        {
            return _students.Where(s => s.Grades.Average() >= min);
        }

        public IEnumerable<Student> GetStudentsOrderedByName()
        {
            return _students.OrderBy(s => s.Name);
        }

        public ILookup<string, Student> GroupStudentsByFaculty()
        {
            return _students.ToLookup(s => s.Faculty);
        }

        public string GetFacultyWithHighestAverageGrade()
        {
            return _students.GroupBy(s => s.Faculty).Select(g => new{Faculty = g.Key,Avg = g.Average(s => s.Grades.Average())}).OrderByDescending(x => x.Avg).FirstOrDefault()?.Faculty;
        }
    }
}