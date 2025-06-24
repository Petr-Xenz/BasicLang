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
                Skip();
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
                Skip();
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
                Skip();
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
                Skip();
                var right = ParseEquality(Peek());
                return new AndExpression(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition));
            }

            return left;
        }

        private IExpression ParseEquality(Token current)
        {
            var left = ParseComparison(current);

            while (Match(Equal, NotEqual))
            {
                var op = Consume();
                var right = ParseComparison(Peek());
                return op.Type switch
                {
                    Equal => new EqualityExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    NotEqual => new NonEqualityExpressions(left, right, GetSourcePositionFromRange(left.SourcePosition, right.SourcePosition)),
                    _ => throw new ProgramException(op.Type.ToString(), op.SourcePosition)
                };
            }

            return left;
        }

        private IExpression ParseComparison(Token current)
        {
            var left = ParseTerm(current);

            while (Match(LessThen, LessThenOrEqual, GreaterThen, GreaterThenOrEqual))
            {
                var op = Consume();
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
                var op = Consume();
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
                var op = Consume();
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
            Skip();
            return current.Type switch
            {
                Number => ParseNumber(),
                Identifier => ParseIdentifier(),
                True => new BooleanExpression(true, current.SourcePosition),
                False => new BooleanExpression(false, current.SourcePosition),
                TokenType.String => new StringExpression(current),
                OpenParenthesis => ParseGrouping(current),
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

            IExpression ParseIdentifier()
            {
                return Peek().Type switch
                {
                    OpenParenthesis => ParseFunctionCall(),
                    _ => new VariableExpression(current.Value, current.SourcePosition),
                };
            }

            IExpression ParseFunctionCall()
            {
                var name = current;
                Skip(); // (
                var parameters = !Match(CloseParenthesis) ? _parser.ParseExpressionsList(Peek()) : Enumerable.Empty<IExpression>();
                _parser.Expect(CloseParenthesis);
                var end = Consume(); // )

                return new FunctionCallExpression(name.Value, parameters, GetSourcePositionFromRange(name, end));
            }

            IExpression ParseGrouping(Token starting)
            {
                var inner = Parse(Peek());
                if (!Match(CloseParenthesis))
                {
                    throw new ProgramException(") expected", Peek().SourcePosition);
                }
                var last = Consume(); // )

                return new GroupingExpression(inner, GetSourcePositionFromRange(starting, last));
            }
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
