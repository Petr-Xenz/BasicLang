namespace BasicLang.AbstractTree.Statements.Expressions;

internal class FunctionCallExpression : IExpression
{
    public FunctionCallExpression(string value, IEnumerable<IExpression> parameters, SourcePosition sourcePosition)
    {
        Value = value;
        Parameters = parameters;
        SourcePosition = sourcePosition;
    }

    public string Name => Value;

    public IEnumerable<IExpression> Children => [];

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value { get; }

    public IEnumerable<IExpression> Parameters { get; }

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}