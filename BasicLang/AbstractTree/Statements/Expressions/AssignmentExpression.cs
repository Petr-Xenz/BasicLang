using System.Reflection.Metadata;

namespace BasicLang.AbstractTree.Statements.Expressions;

internal class AssignmentExpression : BinaryExpression
{
    public AssignmentExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {

    }

    protected override string Delimeter => "=";
}
