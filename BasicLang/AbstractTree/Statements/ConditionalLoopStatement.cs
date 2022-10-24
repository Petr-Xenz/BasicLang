namespace BasicLang.AbstractTree.Statements;

internal class ConditionalLoopStatement : IStatement
{
    public ConditionalLoopStatement(IExpression condition, IEnumerable<IStatement> innerStatements, SourcePosition sourcePosition)
    {
        Condition = condition;
        InnerStatements = innerStatements;
        SourcePosition = sourcePosition;
        Value = $"";//TODO
    }

    public IEnumerable<IStatement> Children
    {
        get
        {
            yield return Condition;
            foreach (var s in InnerStatements)
            {
                yield return s;
            }
        }
    }

    public IExpression Condition { get; }

    public SourcePosition GeneralErrorPosition { get; protected set; }

    public IEnumerable<IStatement> InnerStatements { get; }

    public SourcePosition SourcePosition { get; }

    public string Value { get; }
}