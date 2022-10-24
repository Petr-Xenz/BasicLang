namespace BasicLang.AbstractTree.Statements;

internal class ForStatement : IStatement
{
    public ForStatement(ForCounterExpression? counter, IEnumerable<IStatement> innerStatements, SourcePosition sourcePosition)
    {
        GeneralErrorPosition = new SourcePosition(sourcePosition.Offset, sourcePosition.Line, sourcePosition.Column, 3);
        Value = $"";//TODO
        Counter = counter;
        InnerStatements = innerStatements;
        SourcePosition = sourcePosition;
    }

    public SourcePosition GeneralErrorPosition { get; }

    public string Value { get; }

    public IEnumerable<IStatement> Children
    {
        get
        {
            if (Counter is not null)
            {
                yield return Counter;
            }
            foreach (var s in InnerStatements)
            {
                yield return s;
            }
        }
    }

    public ForCounterExpression? Counter { get; }

    public IEnumerable<IStatement> InnerStatements { get; }

    public SourcePosition SourcePosition { get; }
}