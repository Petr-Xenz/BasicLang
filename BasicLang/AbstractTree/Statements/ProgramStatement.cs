namespace BasicLang.AbstractTree.Statements;

internal class ProgramStatement : IStatement
{
    public ProgramStatement(IEnumerable<IStatement> children, SourcePosition sourcePosition)
    {
        Children = children;
        SourcePosition = sourcePosition;
    }

    public SourcePosition GeneralErrorPosition => new(SourcePosition.Offset, SourcePosition.Line, SourcePosition.Column, 7);

    public string Value { get; } = "TODO";

    public IEnumerable<IStatement> Children { get; }

    public SourcePosition SourcePosition { get; }
}
