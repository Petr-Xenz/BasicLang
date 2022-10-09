namespace BasicLang.AbstractTree.Statements;

internal class IfStatement : IStatement
{
    public IfStatement(IExpression condition, IStatement onTrue, IStatement? onFalse, IEnumerable<ElseIfStatement> elseIfs, SourcePosition sourcePosition)
    {
        GeneralErrorPosition = new SourcePosition(sourcePosition.Offset, sourcePosition.Line, sourcePosition.Column, 2);
        Value = $"if {condition.Value} then {onTrue.Value}" 
            + elseIfs.Select(s => s.Value).Aggregate("", (p, c) => $"{Environment.NewLine}{p}{c}")
            + (onFalse is null ? "" : $"{Environment.NewLine}else {onFalse.Value}");
        Condition = condition;
        OnTrue = onTrue;
        OnFalse = onFalse;
        ElseIfStatements = elseIfs;
        SourcePosition = sourcePosition;
    }

    public IExpression Condition { get; }

    public IStatement OnTrue { get; }

    public IStatement? OnFalse { get; }
    
    public IEnumerable<ElseIfStatement> ElseIfStatements { get; }
    
    public SourcePosition GeneralErrorPosition { get; }

    public string Value { get; }

    public IEnumerable<IStatement> Children
    {
        get
        {
            yield return Condition;
            yield return OnTrue;
            foreach (var s in ElseIfStatements)
            {
                yield return s;
            }
            if (OnFalse is not null)
                yield return OnFalse;
        }
    }

    public SourcePosition SourcePosition { get; }
}
