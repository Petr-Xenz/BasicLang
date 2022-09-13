namespace BasicLang.AbstractTree.Statements
{
    internal class ProgramStatement : IStatement
    {
        public ProgramStatement(string value, IEnumerable<IStatement> children, SourcePosition sourcePosition)
        {
            Value = value;
            Children = children;
            SourcePosition = sourcePosition;
        }

        public SourcePosition GeneralErrorPosition => new(SourcePosition.Offset, SourcePosition.Line, SourcePosition.Column, 7);

        public string Value { get; }

        public IEnumerable<IStatement> Children { get; }

        public SourcePosition SourcePosition { get; }
    }
}
