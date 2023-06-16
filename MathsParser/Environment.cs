namespace MathsParser;

public class Environment
{
    public Environment(Dictionary<string, Delegate> functions, Dictionary<string, double> variables)
    {
        Functions = functions;
        Variables = variables;
    }

    public Dictionary<string, Delegate> Functions { get; }
    public Dictionary<string, double> Variables { get; }
}