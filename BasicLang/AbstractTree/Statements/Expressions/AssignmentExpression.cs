namespace BasicLang.AbstractTree.Statements.Expressions;

internal class AssignmentExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    : BinaryExpression(left, right, sourcePosition)
{
    protected override string Delimiter => "=";
}
