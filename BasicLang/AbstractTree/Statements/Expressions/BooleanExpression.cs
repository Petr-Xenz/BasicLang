namespace BasicLang.AbstractTree.Statements.Expressions;

internal class BooleanExpression : IExpression
{
    public BooleanExpression(string value, SourcePosition sourcePosition)
    {
        Value = value;
        LiteralValue = bool.Parse(value);
        SourcePosition = sourcePosition;
    }

    public bool LiteralValue { get; }

    public IEnumerable<IExpression> Children => Enumerable.Empty<IExpression>();

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value { get; }

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}
