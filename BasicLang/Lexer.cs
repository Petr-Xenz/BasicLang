using BasicLang.AbstractTree;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang;

internal class Lexer
{
    private readonly string _source;

    private int _position;

    private int _columnPosition = 1;

    private int _currentLine = 1;

    private static int _newLineLength = Environment.NewLine.Length;

    private static IReadOnlyDictionary<char, TokenType> _simpleOperatorsToType = new Dictionary<char, TokenType>
    {
        { '<', LessThen },
        { '>', GreaterThen },
        { '+', Addition },
        { '-', Subtraction },
        { '*', Multiplication },
        { '/', Division },
        { '=', Assignment},
        { '(', OpenParenthesis },
        { ')', CloseParenthesis },
        { ',', Comma},
        { ';', Semicolon},
        { ':', Colon },
    };

    public Lexer(string source)
    {
        _source = source;
    }

    public IEnumerable<Token> Lex()
    {
        while (_position < _source.Length)
        {
            if (IsEoL())
            {
                yield return new Token(EoL, _currentLine, _columnPosition, _newLineLength, Environment.NewLine);
                MoveToNewLine();
            }
            else if (char.IsNumber(Peek()))
            {
                yield return TryLexNumber();
                
            }
            else if (Peek() == '"')
            {
                var startingPosition = _position;
                _position++;
                _currentLine++;
                MoveWhile(c => c != '\n' || c != '\n');
                // TODO error reporting
                yield return new Token(TokenType.String, _columnPosition, _currentLine, _position - startingPosition, _source[(startingPosition + 1)..(_position - 1)]);
            }
            else if (Peek() == '=' && PeekNext() == '=')
            {
                yield return new Token(Equal, _currentLine, _columnPosition, 2, "==");
                _position += 2;
                _columnPosition += 2;
            }
            else if (Peek() == '<' && PeekNext() == '>')
            {
                yield return new Token(NotEqual, _currentLine, _columnPosition, 2, "<>");
                _position += 2;
                _columnPosition += 2;
            }
            else if (Peek() == '>' && PeekNext() == '=')
            {
                yield return new Token(GreaterThenOrEqual, _currentLine, _columnPosition, 2, ">=");
                _position += 2;
                _columnPosition += 2;
            }
            else if (Peek() == '<' && PeekNext() == '=')
            {
                yield return new Token(LessThenOrEqual, _currentLine, _columnPosition, 2, "<=");
                _position += 2;
                _columnPosition += 2;
            }
            else if (_simpleOperatorsToType.TryGetValue(Peek(), out var type))
            {
                yield return new Token(type, _currentLine, _columnPosition, 1, Peek().ToString());
                _position++;
                _columnPosition++;
            }
            else if (char.IsWhiteSpace(Peek()))
            {
                //TODO WST
                _position++;
                _columnPosition++;
                MoveWhile(c => char.IsWhiteSpace(c));
            }
        }

        yield return new Token(EoF, _currentLine, _columnPosition, 0, "");
    }

    private void MoveToNewLine()
    {
        _currentLine++;
        _position += _newLineLength;
        _columnPosition = 1;
    }

    private Token TryLexNumber()
    {
        var startingPosition = _position;
        _position++;
        _currentLine++;
        MoveWhile(CanBeNumber);

        var length = _position - startingPosition;
        return new Token(Number, _currentLine, _columnPosition, length, _source.Substring(startingPosition, length));

        bool CanBeNumber(char current)
        {
            return char.IsDigit(current)
                || current == '.' && char.IsDigit(PeekNext());
        }
    }

    private void MoveWhile(Func<char, bool> predicate)
    {
        while (_position < _source.Length && predicate(Peek()))
        {
            _position++;
            _columnPosition++;
        }
    }

    private char Move()
    {
        if (_position < _source.Length)
        {
            _position++;
            _columnPosition++;
            return _source[_position];
        }

        return '\0';
    }

    private char Peek()
    {
        return _source[_position];
    }

    private char PeekNext()
    {
        return _source.Length >= _position + 1
            ? _source[_position + 1]
            : '\0';
    }

    private bool IsEoL()
    {
        return _newLineLength == 1
            ? _source[_position] == '\n'
            : _source[_position] == '\n' && PeekNext() == '\r';
    }
}
