namespace BasicLang.AbstractTree.Statements;

internal class VariableDeclarationStatement : IStatement
{
    public VariableDeclarationStatement(ExpressionStatement expression, string variableName, SourcePosition generalErrorPosition, SourcePosition sourcePosition)
    {
        Expression = expression;
        VariableName = variableName;
        GeneralErrorPosition = generalErrorPosition;
        Value = $"let {expression.Value}";
        SourcePosition = sourcePosition;
    }

    public ExpressionStatement Expression { get; }

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
