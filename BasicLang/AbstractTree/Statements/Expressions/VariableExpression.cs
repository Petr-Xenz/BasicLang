namespace BasicLang.AbstractTree.Statements.Expressions;

internal class VariableExpression : IExpression
{
    public VariableExpression(string value, SourcePosition sourcePosition)
    {
        Value = value;
        SourcePosition = sourcePosition;
    }

    public string Name => Value;

    public IEnumerable<IExpression> Children => [];

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value { get; }

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}