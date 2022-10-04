namespace BasicLang.AbstractTree.Statements;

internal class GotoStatement : IStatement
{
    public GotoStatement(string lineValue, SourcePosition sourcePosition)
    {
        LineValue = lineValue;
        SourcePosition = sourcePosition;
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => $"Goto {LineValue}";

    public IEnumerable<IStatement> Children { get { yield break; } }

    public SourcePosition SourcePosition { get; }

    public string LineValue { get; }
}
