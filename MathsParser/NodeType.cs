namespace MathsParser;

public enum NodeType
{
    Binary,
    Call,
    Number,
    Identifier,
    Expression,
    Abs // TODO: is this the best way to do this?
}