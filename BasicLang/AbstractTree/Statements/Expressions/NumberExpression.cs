namespace BasicLang.AbstractTree.Statements.Expressions;

internal abstract class NumberExpression<T> : IExpression where T : struct
{

    public NumberExpression(T value, SourcePosition sourcePosition)
    {
        LiteralValue = value;
        SourcePosition = sourcePosition;

    }

    public IEnumerable<IExpression> Children
    {
        get
        {
            yield break;
        }
    }

    public T LiteralValue { get; }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => LiteralValue.ToString() ?? "";

    public SourcePosition SourcePosition { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}

internal class FloatLiteralExpression(double value, SourcePosition sourcePosition)
    : NumberExpression<double>(value, sourcePosition);

internal class IntegerLiteralExpression(long value, SourcePosition sourcePosition)
    : NumberExpression<long>(value, sourcePosition);
