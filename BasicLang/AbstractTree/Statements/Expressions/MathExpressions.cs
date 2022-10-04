namespace BasicLang.AbstractTree.Statements.Expressions;

internal class AdditionExpression : BinaryExpression
{
    public AdditionExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {

    }

    protected override string Delimeter => "+";
}

internal class SubtractionExpression : BinaryExpression
{
    public SubtractionExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {

    }

    protected override string Delimeter => "-";
}

internal class MultiplicationExpression : BinaryExpression
{
    public MultiplicationExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {

    }

    protected override string Delimeter => "*";
}

internal class DivisionExpression : BinaryExpression
{
    public DivisionExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
    {

    }

    protected override string Delimeter => "/";
}
