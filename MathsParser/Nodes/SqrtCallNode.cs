﻿namespace MathsParser.Nodes;

public class SqrtCallNode : CallNode
{
    public SqrtCallNode(Node[] arguments) : base(new IdentifierNode("sqrt"), arguments)
    {
    }

    public override NodeType Type => NodeType.Call;

    public override string ToString()
    {
        var args = "";
        foreach (var arg in Arguments) args += $"{arg}, ";
        var argsString = args.Length > 0 ? args[..^2] : "";
        return $"√({argsString})";
    }
}