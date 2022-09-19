namespace BasicLang.AbstractTree.Statements.Expressions
{
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

        public string Value => LiteralValue.ToString() ?? "" ;

        public SourcePosition SourcePosition { get; }

        IEnumerable<IStatement> IStatement.Children => Children;
    }

    internal class FloatLiteralExpression : NumberExpression<double>
    {
        public FloatLiteralExpression(double value, SourcePosition sourcePosition) : base(value, sourcePosition)
        {
        }
    }

    internal class IntegerLiteralExpression : NumberExpression<long>
    {
        public IntegerLiteralExpression(long value, SourcePosition sourcePosition) : base(value, sourcePosition)
        {
        }
    }
}
