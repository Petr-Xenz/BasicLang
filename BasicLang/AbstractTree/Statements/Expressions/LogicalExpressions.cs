using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLang.AbstractTree.Statements.Expressions;

internal class OrExpression : BinaryExpression
{
    public OrExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => "or";
}


internal class XorExpression : BinaryExpression
{
    public XorExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => "xor";
}

internal class AndExpression : BinaryExpression
{
    public AndExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {
    }

    protected override string Delimeter => "and";
}

internal class NotExpression : IExpression
{
    public IExpression Inner { get; }

    public NotExpression(IExpression inner, SourcePosition sourcePosition)
    {
        Inner = inner;
        SourcePosition = sourcePosition;
    }

    public IEnumerable<IExpression> Children
    {
        get
        {
            yield return Inner;
        }
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;
    
    public string Value => $"not {Inner.Value}";

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}
