namespace BasicLang.AbstractTree.Statements;

internal class ForCounterExpression : IExpression
{
    public ForCounterExpression(IExpression counterVariable, IExpression limit, long step, SourcePosition sourcePosition)
    {
        CounterVariable = counterVariable;
        Limit = limit;
        Step = step;
        SourcePosition = sourcePosition;
    }

    public IEnumerable<IExpression> Children
    {
        get
        {
            yield return CounterVariable;
            yield return Limit;
        }
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => "TODO";

    public SourcePosition SourcePosition { get; }
    public IExpression CounterVariable { get; }
    public IExpression Limit { get; }
    public long Step { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}