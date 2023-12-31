﻿namespace MathsParser.Nodes;

public class NumberNode : Node
{
    public NumberNode(Token token)
    {
        Value = double.Parse(token.Value);
    }

    public NumberNode(string value)
    {
        Value = double.Parse(value);
    }

    public NumberNode(double value)
    {
        Value = value;
    }

    public override NodeType Type => NodeType.Number;

    public double Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override double Evaluate(Environment environment = null)
    {
        return Value;
    }
}