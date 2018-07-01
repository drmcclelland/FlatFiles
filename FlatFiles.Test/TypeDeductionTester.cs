﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlatFiles.TypeMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlatFiles.Test
{
    [TestClass]
    public class TypeDeductionTester
    {
        [TestMethod]
        public void ShouldDeduceSchemaForType()
        {
            const string data = @"Id,Name,CreatedOn,IsActive,VisitCount
1,Bob,2018-07-01,true,1
2,John,2018-07-2,false,
3,Susan,2018-07-03,false,10
";
            var stringReader = new StringReader(data);
            var reader = SeparatedValueTypeMapper.GetAutoMappedReader<Person>(stringReader);
            var results = reader.ReadAll().ToArray();
            Assert.AreEqual(3, results.Length, "The wrong number of records were read.");
            var expected = new[]
            {
                new Person() { Id = 1, Name = "Bob", CreatedOn = new DateTime(2018, 07, 01), IsActive = true, VisitCount = 1 },
                new Person() { Id = 2, Name = "John", CreatedOn = new DateTime(2018, 07, 02), IsActive = false, VisitCount = null },
                new Person() { Id = 3, Name = "Susan", CreatedOn = new DateTime(2018, 07, 03), IsActive = false, VisitCount = 10 }
            };
            AssertEqual(expected, results, 0);
            AssertEqual(expected, results, 1);
            AssertEqual(expected, results, 2);
        }

        [TestMethod]
        public async Task ShouldDeduceSchemaForTypeAsync()
        {
            const string data = @"Id,Name,CreatedOn,IsActive,VisitCount
1,Bob,2018-07-01,true,1
2,John,2018-07-2,false,
3,Susan,2018-07-03,false,10
";
            var stringReader = new StringReader(data);
            var reader = await SeparatedValueTypeMapper.GetAutoMappedReaderAsync<Person>(stringReader);
            var results = new List<Person>();
            while (await reader.ReadAsync())
            {
                results.Add(reader.Current);
            }
            Assert.AreEqual(3, results.Count, "The wrong number of records were read.");
            var expected = new[]
            {
                new Person() { Id = 1, Name = "Bob", CreatedOn = new DateTime(2018, 07, 01), IsActive = true, VisitCount = 1 },
                new Person() { Id = 2, Name = "John", CreatedOn = new DateTime(2018, 07, 02), IsActive = false, VisitCount = null },
                new Person() { Id = 3, Name = "Susan", CreatedOn = new DateTime(2018, 07, 03), IsActive = false, VisitCount = 10 }
            };
            AssertEqual(expected, results, 0);
            AssertEqual(expected, results, 1);
            AssertEqual(expected, results, 2);
        }

        private static void AssertEqual(IList<Person> expected, IList<Person> actual, int id)
        {
            Assert.AreEqual(expected[id].Id, actual[id].Id, $"Wrong ID for person {id}");
            Assert.AreEqual(expected[id].Name, actual[id].Name, $"Wrong Name for person {id}");
            Assert.AreEqual(expected[id].CreatedOn, actual[id].CreatedOn, $"Wrong CreatedOn for person {id}");
            Assert.AreEqual(expected[id].IsActive, actual[id].IsActive, $"Wrong IsActive for person {id}");
            Assert.AreEqual(expected[id].VisitCount, actual[id].VisitCount, $"Wrong VisitCount for person {id}");
        }

        internal class Person
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime CreatedOn { get; set; }

            public bool IsActive { get; set; }

            public int? VisitCount { get; set; }
        }
    }
}