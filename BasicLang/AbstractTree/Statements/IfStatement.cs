namespace BasicLang.AbstractTree.Statements;

internal class IfStatement : IStatement
{
    public IfStatement(IExpression condition, IStatement onTrue, IStatement? onFalse, SourcePosition sourcePosition)
    {
        GeneralErrorPosition = new SourcePosition(sourcePosition.Offset, sourcePosition.Line, sourcePosition.Column, 2);
        Value = $"if {condition.Value} then {onTrue.Value}" + (onFalse is null ? "" : $"else {onFalse.Value}");
        Condition = condition;
        OnTrue = onTrue;
        OnFalse = onFalse;
        SourcePosition = sourcePosition;
    }

    public IExpression Condition { get; }
    public IStatement OnTrue { get; }
    public IStatement? OnFalse { get; }

    public SourcePosition GeneralErrorPosition { get; }

    public string Value { get; }

    public IEnumerable<IStatement> Children 
    { 
        get 
        { 
            yield return Condition;
            yield return OnTrue;
            if (OnFalse is not null)
                yield return OnFalse; 
        } 
    }

    public SourcePosition SourcePosition { get; }
}