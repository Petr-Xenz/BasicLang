using BasicLang.AbstractTree;
using BasicLang.AbstractTree.Statements.Expressions;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang.Parsing
{
    internal partial class Parser
    {
        private class ExpressionParser
        {
            private readonly Parser _parser;

            public ExpressionParser(Parser parser)
            {
                _parser = parser;
            }

            public IExpression Parse(Token current) => ParserAssignment(current);

            private IExpression ParserAssignment(Token current)
            {
                var left = ParseOr(current);

                while (Match(Assignment))
                {
                    Skip(2);
                    var right = ParseOr(Peek());
                    var assignment = new AssignmentExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition));
                    return assignment;
                }

                return left;
            }

            private IExpression ParseOr(Token current)
            {
                //TODO
                return ParseXor(current);
            }

            private IExpression ParseXor(Token current)
            {
                //TODO
                return ParseAnd(current);
            }

            private IExpression ParseAnd(Token current)
            {
                //TODO
                return ParseEquality(current);
            }

            private IExpression ParseEquality(Token current)
            {
                //TODO
                return ParseComparsion(current);
            }

            private IExpression ParseComparsion(Token current)
            {
                //TODO
                return ParseTerm(current);
            }

            private IExpression ParseTerm(Token current)
            {
                //TODO
                return ParseFactor(current);
            }

            private IExpression ParseFactor(Token current)
            {
                //TODO
                return ParseUnary(current);
            }

            private IExpression ParseUnary(Token current)
            {
                //TODO
                return ParsePrimary(current);
            }

            private IExpression ParsePrimary(Token current)
            {
                return current.Type switch
                {
                    Number => ParseNumber(),
                    Identifier => ParseIdentifier(),
                    _ => throw new ProgramException($"Unexpected token {current.Type}", current.SourcePosition),
                };

                IExpression ParseNumber()
                {
                    if (current.Value.Contains('.'))
                    {
                        return new FloatLiteralExpression(double.Parse(current.Value), current.SourcePosition);
                    }
                    else
                    {
                        return new IntegerLiteralExpression(long.Parse(current.Value), current.SourcePosition);
                    }
                }

                IExpression ParseIdentifier() => new VariableExpression(current.Value, current.SourcePosition);
            }

            private bool Match(TokenType type) => _parser.Match(type);

            private void Skip(int step = 1) => _parser.Skip(step);

            private Token Peek() => _parser.Peek();

            private SourcePosition GetSourcePositionFromRange(SourcePosition start, SourcePosition end) =>
                _parser.GetSourcePositionFromRange(start, end);

        }
    }
}
