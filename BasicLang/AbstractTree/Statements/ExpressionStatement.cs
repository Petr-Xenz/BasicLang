namespace BasicLang.AbstractTree.Statements
{
    internal class ExpressionStatement : IStatement
    {
        public ExpressionStatement(IExpression child)
        {
            Child = child;
        }

        public IExpression Child { get; }

        public SourcePosition GeneralErrorPosition => Child.GeneralErrorPosition;

        public string Value => Child.Value;

        public SourcePosition SourcePosition => Child.SourcePosition;

        public IEnumerable<IStatement> Children
        {
            get
            {
                yield return Child;
            }
        }
    }
}
