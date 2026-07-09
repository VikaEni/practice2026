using task11;
using Xunit;

namespace task11tests
{
    public class CalculatorTests
    {
        [Fact]
        public void CreateCalculator_ShouldReturnICalculator()
        {
            var generator = new CodeGenerator();
            var calculator = generator.CreateCalculator();

            Assert.NotNull(calculator);
            Assert.IsAssignableFrom<ICalculator>(calculator);
        }

        [Fact]
        public void Add_ShouldReturnSum()
        {
            var generator = new CodeGenerator();
            var calc = generator.CreateCalculator();

            var result = calc.Add(5, 3);

            Assert.Equal(8, result);
        }

        [Fact]
        public void Minus_ShouldReturnDifference()
        {
            var generator = new CodeGenerator();
            var calc = generator.CreateCalculator();

            var result = calc.Minus(10, 4);

            Assert.Equal(6, result);
        }

        [Fact]
        public void Mul_ShouldReturnProduct()
        {
            var generator = new CodeGenerator();
            var calc = generator.CreateCalculator();

            var result = calc.Mul(4, 5);

            Assert.Equal(20, result);
        }

        [Fact]
        public void Div_ShouldReturnQuotient()
        {
            var generator = new CodeGenerator();
            var calc = generator.CreateCalculator();

            var result = calc.Div(15, 3);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Div_ShouldThrowDivideByZeroException()
        {
            var generator = new CodeGenerator();
            var calc = generator.CreateCalculator();

            Assert.Throws<DivideByZeroException>(() => calc.Div(10, 0));
        }
    }
}