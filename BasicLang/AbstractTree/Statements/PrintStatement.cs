namespace BasicLang.AbstractTree.Statements;

internal class PrintStatement : IStatement
{
    public PrintStatement(IEnumerable<IExpression> children, SourcePosition sourcePosition)
    {
        Expressions = children;
        SourcePosition = sourcePosition;
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => "print " + Expressions.Select(c => c.Value).Aggregate((p, c) => $"{p}, {c}");

    public IEnumerable<IStatement> Children => Expressions;

    public IEnumerable<IExpression> Expressions { get; }

    public SourcePosition SourcePosition { get; }
}
