namespace BasicLang.AbstractTree.Statements.Expressions;

internal class StringExpression : IExpression
{
    public StringExpression(Token token)
    {
        Value = token.Value;
        LiteralValue = token.Value[1..^2];
        SourcePosition = token.SourcePosition;
    }

    public IEnumerable<IExpression> Children
    {
        get
        {
            yield break;
        }
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value { get; }

    public string LiteralValue { get; }

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}
