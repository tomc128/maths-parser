namespace MathsParser.Nodes;

internal class ExpressionNode : Node
{
    public ExpressionNode(string expression)
    {
        Expression = expression;
    }

    public override NodeType Type => NodeType.Expression;

    public string Expression { get; }

    public override string ToString()
    {
        return $"\"{Expression}\"";
    }

    public override double Evaluate(Environment environment = null)
    {
        return new Parser().Read(Expression).Evaluate(environment);
    }
}