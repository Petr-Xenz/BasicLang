namespace BasicLang.AbstractTree.Statements;

internal class FunctionStatement : IStatement
{
    public FunctionStatement(Token functionName, IEnumerable<IExpression> arguments, 
        IEnumerable<IStatement> innerStatements, SourcePosition sourcePosition, SourcePosition initialToken)
    {
        FunctionName = functionName.Value;
        Arguments = arguments;
        InnerStatements = innerStatements;
        SourcePosition = sourcePosition;
        GeneralErrorPosition = new SourcePosition(initialToken.Offset, initialToken.Line, 
            initialToken.Column, functionName.SourcePosition.Offset + functionName.SourcePosition.Length - initialToken.Offset);
        Value = "TODO";
    }

    public SourcePosition GeneralErrorPosition { get; }

    public string Value { get; }

    public IEnumerable<IStatement> Children
    {
        get
        {
            foreach (var arg in Arguments)
            {
                yield return arg;
            }

            foreach (var statement in InnerStatements)
            {
                yield return statement;
            }
        }
    }

    public SourcePosition SourcePosition { get; }
    public string FunctionName { get; }
    public IEnumerable<IExpression> Arguments { get; }
    public IEnumerable<IStatement> InnerStatements { get; }
}
