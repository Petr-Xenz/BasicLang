using BasicLang.AbstractTree;
using BasicLang.AbstractTree.Statements.Expressions;
using System.Globalization;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang.Parsing;

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
            var left = ParseXor(current);

            while (Match(Or))
            {
                Skip(2);
                var right = ParseXor(Peek());
                return new OrExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition));
            }

            return left;
        }

        private IExpression ParseXor(Token current)
        {
            var left = ParseAnd(current);

            while (Match(Xor))
            {
                Skip(2);
                var right = ParseAnd(Peek());
                return new XorExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition));
            }

            return left;
        }

        private IExpression ParseAnd(Token current)
        {
            var left = ParseEquality(current);

            while (Match(And))
            {
                Skip(2);
                var right = ParseEquality(Peek());
                return new AndExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition));
            }

            return left;
        }

        private IExpression ParseEquality(Token current)
        {
            var left = ParseComparsion(current);

            while (Match(Equal, NotEqual))
            {
                var op = PeekNext();
                Skip(2);
                var right = ParseComparsion(Peek());
                return op.Type switch
                {
                    Equal => new EqualityExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    NotEqual => new NonEqualityExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    _ => throw new ProgramException(op.Type.ToString(), op.SourcePosition)
                };
            }

            return left;
        }

        private IExpression ParseComparsion(Token current)
        {
            var left = ParseTerm(current);

            while (Match(LessThen, LessThenOrEqual, GreaterThen, GreaterThenOrEqual))
            {
                var op = PeekNext();
                Skip(2);
                var right = ParseTerm(Peek());
                return op.Type switch
                {
                    LessThen => new LessThanExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    LessThenOrEqual => new LessThanOrEqualExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    GreaterThen => new GreaterThanExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    GreaterThenOrEqual => new GreaterThanOrEqualExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    _ => throw new ProgramException(op.Type.ToString(), op.SourcePosition)
                };
            }

            return left;
        }

        private IExpression ParseTerm(Token current)
        {
            var left = ParseFactor(current);

            while (Match(Addition, Subtraction))
            {
                var op = PeekNext();
                Skip(2);
                var right = ParseFactor(Peek());
                return op.Type switch
                {
                    Addition => new AdditionExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    Subtraction => new SubtractionExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    _ => throw new ProgramException(op.Type.ToString(), op.SourcePosition)
                };
            }

            return left;
        }

        private IExpression ParseFactor(Token current)
        {
            var left = ParseUnary(current);

            while (Match(Multiplication, Division))
            {
                var op = PeekNext();
                Skip(2);
                var right = ParseUnary(Peek());
                return op.Type switch
                {
                    Multiplication => new MultiplicationExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    Division => new DivisionExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    _ => throw new ProgramException(op.Type.ToString(), op.SourcePosition)
                };
            }

            return left;
        }

        private IExpression ParseUnary(Token current)
        {
            if (current.Type == Not)
            {
                Skip();
                var next = Peek();
                return new NotExpression(ParsePrimary(next), GetSourcePositionFromRange(current, next));
            }
            return ParsePrimary(current);
        }

        private IExpression ParsePrimary(Token current)
        {
            return current.Type switch
            {
                Number => ParseNumber(),
                Identifier => ParseIdentifier(),
                TokenType.String => new StringExpression(current),
                _ => throw new ProgramException($"Unexpected token {current.Type}", current.SourcePosition),
            };

            IExpression ParseNumber()
            {
                if (current.Value.Contains('.'))
                {
                    return new FloatLiteralExpression(double.Parse(current.Value, CultureInfo.InvariantCulture), current.SourcePosition);
                }
                else
                {
                    return new IntegerLiteralExpression(long.Parse(current.Value), current.SourcePosition);
                }
            }

            IExpression ParseIdentifier() => new VariableExpression(current.Value, current.SourcePosition);
        }

        private bool Match(TokenType type) => _parser.Match(type);

        private bool Match(params TokenType[] types) => _parser.Match(types);

        private void Skip(int step = 1) => _parser.Skip(step);

        private Token Consume() => _parser.Consume();

        private Token Peek() => _parser.Peek();

        private Token PeekNext() => _parser.PeekNext();

        private SourcePosition GetSourcePositionFromRange(SourcePosition start, SourcePosition end) =>
            _parser.GetSourcePositionFromRange(start, end);

        private SourcePosition GetSourcePositionFromRange(ICodeElement start, ICodeElement end) =>
            _parser.GetSourcePositionFromRange(start, end);

    }
}
