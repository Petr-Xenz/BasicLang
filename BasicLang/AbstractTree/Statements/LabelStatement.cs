namespace BasicLang.AbstractTree.Statements;

internal class LabelStatement : IStatement
{
    public LabelStatement(string value, string lineValue, SourcePosition sourcePosition)
    {
        Value = value;
        SourcePosition = sourcePosition;
        LineValue = lineValue;
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value { get; }

    public IEnumerable<IStatement> Children => Enumerable.Empty<IStatement>();

    public SourcePosition SourcePosition { get; }

    public string LineValue { get; }
}
