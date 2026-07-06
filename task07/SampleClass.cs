using System;

namespace task07
{
    [DisplayName("Sample class")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Number property")]
        public int Number { get; set; }

        [DisplayName("Test method")]
        public void TestMethod()
        {
        }

        public void MethodWithoutAttribute()
        {
        }
    }
}