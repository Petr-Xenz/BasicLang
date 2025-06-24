namespace BasicLang.AbstractTree.Statements.Expressions;

internal class BooleanExpression : IExpression
{
    public BooleanExpression(bool value, SourcePosition sourcePosition)
    {
        LiteralValue = value;
        SourcePosition = sourcePosition;
    }

    public bool LiteralValue { get; }

    public IEnumerable<IExpression> Children => [];

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => LiteralValue.ToString();

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}
