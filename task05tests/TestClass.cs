using System;
using task05;
using Xunit;

namespace task05tests
{
    public class TestClass
    {
        public int PublicField;
        private string _privateField;
        public int Property { get; set; }
        public void Method() { }
        public int MethodWithParams(string param1, int param2) => 0;
        private void PrivateMethod() { }
    }

    [Serializable]
    public class AttributedClass { }

    public class ClassAnalyzerTests
    {
        [Fact]
        public void GetPublicMethods_ReturnsCorrectMethods()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var methods = analyzer.GetPublicMethods();

            Assert.Contains("Method", methods);
            Assert.Contains("MethodWithParams", methods);
        }

        [Fact]
        public void GetAllFields_IncludesPrivateFields()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var fields = analyzer.GetAllFields();

            Assert.Contains("_privateField", fields);
            Assert.Contains("PublicField", fields);
        }

        [Fact]
        public void GetMethodParams_ReturnsCorrectParameters()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var paramsList = analyzer.GetMethodParams("MethodWithParams");

            Assert.Contains("param1", paramsList);
            Assert.Contains("param2", paramsList);
        }

        [Fact]
        public void GetProperties_ReturnsPublicProperties()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var properties = analyzer.GetProperties();

            Assert.Contains("Property", properties);
        }

        [Fact]
        public void HasAttribute_WorksCorrectly()
        {
            var analyzer1 = new ClassAnalyzer(typeof(AttributedClass));
            var analyzer2 = new ClassAnalyzer(typeof(TestClass));

            Assert.True(analyzer1.HasAttribute<SerializableAttribute>());
            Assert.False(analyzer2.HasAttribute<SerializableAttribute>());
        }
    }
}