namespace BasicLang.AbstractTree.Statements.Expressions;

internal class OrExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "or";
}


internal class XorExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "xor";
}

internal class AndExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "and";
}

internal class NotExpression(IExpression inner, SourcePosition sourcePosition)
    : IExpression
{
    public IExpression Inner { get; } = inner;

    public IEnumerable<IExpression> Children
    {
        get
        {
            yield return Inner;
        }
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => $"not {Inner.Value}";

    public SourcePosition SourcePosition { get; } = sourcePosition;

    IEnumerable<IStatement> IStatement.Children => Children;
}
