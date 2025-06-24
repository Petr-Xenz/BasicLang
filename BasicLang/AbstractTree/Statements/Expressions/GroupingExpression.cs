namespace BasicLang.AbstractTree.Statements.Expressions;

internal class GroupingExpression : IExpression
{
    public GroupingExpression(IExpression inner, SourcePosition sourcePosition)
    {
        Inner = inner;
        SourcePosition = sourcePosition;
    }

    public IExpression Inner { get; }

    public IEnumerable<IExpression> Children
    {
        get
        {
            yield return Inner;
        }
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => $"({Inner.Value})";

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}