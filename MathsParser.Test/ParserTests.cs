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
    public void TestExpression(string input, double expected)
    {
        var result = _parser.Read(input).Evaluate(_environment);
        var number = new Number(result); // Convert to the Number class to handle precision issues

        Assert.That(number.Value, Is.EqualTo(expected));
    }
}