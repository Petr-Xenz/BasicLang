namespace BasicLang.AbstractTree.Statements;

internal class ErrorStatement : IStatement
{
    public ErrorStatement(string value, SourcePosition sourcePosition)
    {
        Value = value;
        SourcePosition = sourcePosition;
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value { get; }

    public IEnumerable<IStatement> Children => Enumerable.Empty<IStatement>();

    public SourcePosition SourcePosition { get; }
}
