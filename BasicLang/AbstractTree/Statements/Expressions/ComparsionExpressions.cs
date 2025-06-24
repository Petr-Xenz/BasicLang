namespace BasicLang.AbstractTree.Statements.Expressions;

internal class EqualityExpressions(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "==";
}

internal class NonEqualityExpressions(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "!=";
}

internal class LessThanExpressions(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "<";
}

internal class GreaterThanExpressions(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => ">";
}

internal class LessThanOrEqualExpressions(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "<=";
}

internal class GreaterThanOrEqualExpressions(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => ">=";
}
