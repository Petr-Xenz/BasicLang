namespace BasicLang.AbstractTree.Statements
{
    internal class VariableDeclaration : IStatement
    {
        public VariableDeclaration(StatementExpression expression, string variableName, SourcePosition generalErrorPosition, string value, SourcePosition sourcePosition)
        {
            Expression = expression;
            VariableName = variableName;
            GeneralErrorPosition = generalErrorPosition;
            Value = value;
            SourcePosition = sourcePosition;
        }

        public StatementExpression Expression { get; }

        public string VariableName { get; }

        public SourcePosition GeneralErrorPosition { get; }

        public string Value { get; }

        public IEnumerable<IStatement> Children
        {
            get
            {
                yield return Expression;
            }
        }

        public SourcePosition SourcePosition { get; }
    }
}
