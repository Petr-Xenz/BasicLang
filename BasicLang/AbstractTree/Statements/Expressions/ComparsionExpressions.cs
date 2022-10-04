using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLang.AbstractTree.Statements.Expressions;

internal class EqualityExpressions : BinaryExpression
{
    public EqualityExpressions(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => "==";
}

internal class NonEqualityExpressions : BinaryExpression
{
    public NonEqualityExpressions(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => "!=";
}

internal class LessThanExpressions : BinaryExpression
{
    public LessThanExpressions(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => "<";
}

internal class GreaterThanExpressions : BinaryExpression
{
    public GreaterThanExpressions(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => ">";
}

internal class LessThanOrEqualExpressions : BinaryExpression
{
    public LessThanOrEqualExpressions(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => "<=";
}

internal class GreaterThanOrEqualExpressions : BinaryExpression
{
    public GreaterThanOrEqualExpressions(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => ">=";
}
