using BasicLang.AbstractTree.Statements.Expressions;

namespace BasicLang.AbstractTree.Statements;

internal class InputStatement : IStatement
{
    public InputStatement(IEnumerable<InputExpression> children, SourcePosition sourcePosition)
    {
        InputExpressions = children;
        SourcePosition = sourcePosition;
    }

    public SourcePosition GeneralErrorPosition => SourcePosition;

    public string Value => "read " + Expressions.Select(c => c.Value).Aggregate((p, c) => $"{p}, {c}");

    public IEnumerable<IStatement> Children => InputExpressions;

    public IEnumerable<IExpression> Expressions => InputExpressions;

    public SourcePosition SourcePosition { get; }
    
    public IEnumerable<InputExpression> InputExpressions { get; }


    public class InputExpression : IExpression
    {
        private readonly IEnumerable<IExpression> _children;
        private readonly StringExpression? _promptExpression;
        private readonly IEnumerable<VariableExpression> _variables;

        private readonly SourcePosition _sourcePosition;

        public InputExpression(StringExpression? promptExpression, IEnumerable<VariableExpression> variables, SourcePosition sourcePosition)
        {

            _promptExpression = promptExpression;
            _variables = variables;
            _children = promptExpression is not null ? variables.Cast<IExpression>().Prepend(promptExpression) : variables;

            Value = promptExpression is not null
                ? variables.Select(c => c.Value).Aggregate(promptExpression.Value, (p, c) => $"{p}, {c}")
                : variables.Select(c => c.Value).Aggregate((p, c) => $"{p}, {c}");

            _sourcePosition = sourcePosition;

        }

        public string Value { get; }

        internal StringExpression? PromptExpression => _promptExpression;

        internal IEnumerable<VariableExpression> Variables => _variables;

        IEnumerable<IExpression> IExpression.Children => _children;

        IEnumerable<IStatement> IStatement.Children => _children;

        SourcePosition IStatement.GeneralErrorPosition => _sourcePosition;

        SourcePosition ICodeElement.SourcePosition => _sourcePosition;
    }
}
