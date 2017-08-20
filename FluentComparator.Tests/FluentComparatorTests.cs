using FluentAssertions;
using FluentComparator.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentComparator.Tests
{
    [TestClass]
    public class FluentComparatorTests
    {
        private School bme = new School();
        private School bmeCopy = new School();
        private School elte = new School();

        [TestInitialize]
        public void Init()
        {
            bme = new School
            {
                Established = new DateTime(1972, 1, 1, 0, 0, 0),
                Name = "BUDAPEST UNIVERSITY OF TECHNOLOGY AND ECONOMICS",
                NumberOfStudent = 9001,
                Type = SchoolType.Public
            };
            bmeCopy = new School
            {
                Established = new DateTime(1972, 1, 1, 0, 0, 0),
                Name = "BUDAPEST UNIVERSITY OF TECHNOLOGY AND ECONOMICS",
                NumberOfStudent = 9001,
                Type = SchoolType.Public
            };
            elte = new School
            {
                Established = new DateTime(1635, 1, 1, 0, 0, 0),
                Name = "Eötvös University",
                NumberOfStudent = 8999,
                Type = SchoolType.Public
            };
        }


        [TestMethod]
        public void CompareObjects_SimpeProperties_1_Property_Excluded()
        {
            // Act
            var compareResult = Comparator.Create<School>()
                .Compare(bme)
                .To(elte)
                .ExcludeProperty(u => u.NumberOfStudent)
                .EnableDifferences()
                .Evaluate();

            // Assert
            compareResult.IsEquivalent.Should().BeFalse();
            compareResult.Differences.Select(d => d.Name).Contains("Name").Should().BeTrue();
            compareResult.Differences.Select(d => d.Name).Contains("Established").Should().BeTrue();
        }

        [TestMethod]
        public void CompareObjects_SimpeProperties_1_PropertyFromNestedObjectExcluded()
        {
            // TODO: Excluding nested object properties
            //// Arrange
            //bme.BestStudent = new Student
            //{
            //    GPA = 1.0f,
            //    Active = false,
            //    Name = "Smart Guy",
            //    Age = 24
            //};
            //bmeCopy.BestStudent = new Student
            //{
            //    GPA = 1.0f,
            //    Active = true,
            //    Name = "Smart Guy",
            //    Age = 24
            //};

            //// Act
            //var compareResult = Comparator.Create<School>()
            //    .Compare(bme)
            //    .To(bmeCopy)
            //    .ExcludeProperty(u => u.BestStudent.Active)
            //    .ShowDifferences()
            //    .Evaluate();

            //// Assert
            //compareResult.IsEquivalent.Should().BeTrue();
            //compareResult.Differences.Should().BeNull();
        }


        [TestMethod]
        public void CompareObjects_With_NesteObject()
        {
            // Arrange
            bme.BestStudent = new Student
            {
                GPA = 1.0f,
                Active = false,
                Name = "Smart Guy",
                Age = 24
            };

            // Act
            var compareResult = Comparator.Create<School>()
                .Compare(bme)
                .To(elte)
                .EnableDifferences()
                .Evaluate();

            // Assert
            compareResult.IsEquivalent.Should().BeFalse();
            compareResult.Differences.Select(d => d.Name).Contains("Name").Should().BeTrue();
            compareResult.Differences.Select(d => d.Name).Contains("Established").Should().BeTrue();
            compareResult.Differences.Select(d => d.Name).Contains("NumberOfStudent").Should().BeTrue();
            compareResult.Differences.Select(d => d.Name).Contains("BestStudent").Should().BeTrue();

            compareResult.Differences.SingleOrDefault(d => d.Name == "BestStudent").A.Should().NotBeNull();
            compareResult.Differences.SingleOrDefault(d => d.Name == "BestStudent").B.Should().BeNull();
        }

        [TestMethod]
        public void CompareObjects_With_Collections()
        {
            // Arrange
            bme.Students = new List<Student>
            {
                new Student
                {
                    GPA = 1.0f,
                    Active = false,
                    Name = "Smart Guy",
                    Age = 24
                }
            };
            // Arrange
            elte.Students = new List<Student>
            {
                new Student
                {
                    GPA = 1.0f,
                    Active = false,
                    Name = "Other Smart Guy",
                    Age = 23
                }
            };

            // Act
            var compareResult = Comparator.Create<School>()
                .Compare(bme)
                .To(elte)
                .EnableDifferences()
                .Evaluate();

            // Assert
            compareResult.IsEquivalent.Should().BeFalse();
            compareResult.Differences.Select(d => d.Name).Contains("Name").Should().BeTrue();
            compareResult.Differences.Select(d => d.Name).Contains("Established").Should().BeTrue();
            compareResult.Differences.Select(d => d.Name).Contains("NumberOfStudent").Should().BeTrue();
            compareResult.Differences.Select(d => d.Name).Contains("Students").Should().BeTrue();
        }

        [TestMethod]
        public void CompareObjects_HideDIfferences()
        {
            // Act
            var compareResult = Comparator.Create<School>()
                .Compare(bme)
                .To(elte)
                .DisableDifferences()
                .Evaluate();

            // Assert
            compareResult.IsEquivalent.Should().BeFalse();
            compareResult.Differences.Should().BeNull();
        }
    }
}
