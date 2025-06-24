namespace BasicLang.AbstractTree.Statements.Expressions;

internal class AdditionExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "+";
}

internal class SubtractionExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "-";
}

internal class MultiplicationExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "*";
}

internal class DivisionExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "/";
}
