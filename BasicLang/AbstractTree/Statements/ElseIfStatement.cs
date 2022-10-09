namespace BasicLang.AbstractTree.Statements;

internal class ElseIfStatement : IStatement
{
    public ElseIfStatement(IExpression condition, IStatement onTrue, SourcePosition sourcePosition)
    {
        GeneralErrorPosition = new SourcePosition(sourcePosition.Offset, sourcePosition.Line, sourcePosition.Column, 2);
        Value = $"elseif {condition.Value} then {onTrue.Value}";
        Condition = condition;
        OnTrue = onTrue;
        SourcePosition = sourcePosition;
    }

    public IExpression Condition { get; }

    public IStatement OnTrue { get; }

    public SourcePosition GeneralErrorPosition { get; }

    public string Value { get; }

    public IEnumerable<IStatement> Children
    {
        get
        {
            yield return Condition;
            yield return OnTrue;
        }
    }

    public SourcePosition SourcePosition { get; }
}