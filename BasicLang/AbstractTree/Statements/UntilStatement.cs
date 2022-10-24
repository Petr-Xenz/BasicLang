namespace BasicLang.AbstractTree.Statements;

internal class UntilStatement : ConditionalLoopStatement
{
    public UntilStatement(IExpression condition, IEnumerable<IStatement> innerStatements, SourcePosition sourcePosition)
        : base(condition, innerStatements, sourcePosition)
    {
        GeneralErrorPosition = new SourcePosition(sourcePosition.Offset, sourcePosition.Line, sourcePosition.Column, 5);
    }
}