namespace BasicLang.AbstractTree.Statements.Expressions;

internal abstract class BinaryExpression : IExpression
{
    protected abstract string Delimiter { get; }

    public BinaryExpression(IExpression left, IExpression right, SourcePosition sourcePosition)
    {
        Left = left;
        Right = right;
        SourcePosition = sourcePosition;
    }

    public IEnumerable<IExpression> Children
    {
        get
        {
            yield return Left;
            yield return Right;
        }
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public SourcePosition SourcePosition { get; }

    public string Value => $"{Left.Value} {Delimiter} {Right.Value}";

    internal IExpression Left { get; }

    internal IExpression Right { get; }

    IEnumerable<IStatement> IStatement.Children => Children;
}