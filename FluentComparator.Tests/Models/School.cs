using System;
using System.Collections.Generic;

namespace FluentComparator.Tests.Models
{
    public class School
    {
        public string Name { get; set; }
        public DateTime Established { get; set; }
        public int NumberOfStudent { get; set; }
        public SchoolType Type { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public Student BestStudent { get; set; }
    }
}
