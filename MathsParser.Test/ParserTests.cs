namespace MathsParser.Test;

public class ParserTests
{
    private Environment _environment;
    private Parser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new Parser();

        var functions = new Dictionary<string, Delegate>
        {
            { "sin", Math.Sin },
            { "cos", Math.Cos },
            { "tan", Math.Tan },
            { "asin", Math.Asin },
            { "acos", Math.Acos },
            { "atan", Math.Atan },
            { "abs", new Func<double, double>(Math.Abs) },
            { "sqrt", Math.Sqrt },
            { "clamp", new Func<double, double, double, double>(Math.Clamp) },
            { "log", new Func<double, double, double>(Math.Log) },
            { "ln", new Func<double, double>(x => Math.Log(x, Math.E)) },
        };

        var variables = new Dictionary<string, double>
        {
            { "pi", Math.PI },
            { "π", Math.PI },
            { "e", Math.E },
        };

        _environment = new Environment(functions, variables);
    }

    [Test]
    [TestCase("3 pi", 9.42477796076938)]
    [TestCase("2pi", 6.283185307179586)]
    [TestCase("sin 2pi", 0)]
    [TestCase("2 * 2", 4)]
    [TestCase("ln e³", 3)]
    [TestCase("4(2)", 8)]
    [TestCase("clamp(4, 2, 3)", 3)]
    [TestCase("2 * sin pi", 0)]
    [TestCase("2 + 3 * 4", 14)]
    [TestCase("abs -5", 5)]
    [TestCase("3", 3)]
    [TestCase("0", 0)]
    [TestCase("-5", -5)]
    [TestCase("pi", Math.PI)]
    [TestCase("sin(pi/2)", 1)]
    [TestCase("sqrt(16)", 4)]
    [TestCase("ln(e)", 1)]
    [TestCase("abs(-3)", 3)]
    [TestCase("clamp(5, 2, 8)", 5)]
    [TestCase("log(100, 10)", 2)]
    [TestCase("2^3", 8)]
    [TestCase("3 + 4", 7)]
    [TestCase("2 - 5", -3)]
    [TestCase("6 * 7", 42)]
    [TestCase("8 / 2", 4)]
    [TestCase("5 + 3 * 2", 11)]
    [TestCase("(4 + 2) * 3", 18)]
    [TestCase("sin(pi) + cos(pi)", -1)]
    [TestCase("abs(-2) - 1", 1)]
    [TestCase("sqrt(9) + 1.5 * sin(pi/2)", 4.5)]
    [TestCase("3^4^2", 43046721)]
    [TestCase("10 + 2 * 5 - 3 / 2", 18.5)]
    [TestCase("1 + 2 * 3 - 4 / 5 + 6", 12.2)]
    [TestCase("1 / 3 + 2 / 3", 1)]
    [TestCase("1000000000 * 1000000000", 1E+18)]
    [TestCase("log(0, 10)", double.NegativeInfinity)]
    [TestCase("1 / 0", double.PositiveInfinity)]
    [TestCase("sqrt(-1)", double.NaN)]
    public void TestExpression(string input, double expected)
    {
        var result = _parser.Read(input).Evaluate(_environment);
        var number = new Number(result); // Convert to the Number class to handle precision issues

        Assert.That(number.Value, Is.EqualTo(expected));
    }
}