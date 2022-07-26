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
                _currentLine++;
                _position += _newLineLength;
                _columnPosition = 1;
                continue;
            }

            if (char.IsNumber(Peek()))
            {
                yield return TryLexNumber();
                continue;
            }

            if (Peek() == '=' && PeekNext() == '=')
            {
                yield return new Token(Equal, _currentLine, _columnPosition, 2, "==");
                _position += 2;
                _columnPosition += 2;
                continue;
            }

            if (Peek() == '<' && PeekNext() == '>')
            {
                yield return new Token(NotEqual, _currentLine, _columnPosition, 2, "<>");
                _position += 2;
                _columnPosition += 2;
                continue;
            }

            if (Peek() == '>' && PeekNext() == '=')
            {
                yield return new Token(GreaterThenOrEqual, _currentLine, _columnPosition, 2, ">=");
                _position += 2;
                _columnPosition += 2;
                continue;
            }

            if (Peek() == '<' && PeekNext() == '=')
            {
                yield return new Token(LessThenOrEqual, _currentLine, _columnPosition, 2, "<=");
                _position += 2;
                _columnPosition += 2;
                continue;
            }

            if (Peek() == '>')
            {
                yield return new Token(GreaterThen, _currentLine, _columnPosition, 1, ">");
                _position++;
                _columnPosition++;
                continue;
            }

            if (Peek() == '<')
            {
                yield return new Token(LessThen, _currentLine, _columnPosition, 1, "<");
                _position++;
                _columnPosition++;
                continue;
            }


            if (char.IsWhiteSpace(Peek()))
            {
                //TODO WST
                _position++;
                _columnPosition++;
                MoveWhile(c => char.IsWhiteSpace(c));
                continue;
            }
        }

        yield return new Token(EoF, _currentLine, _columnPosition, 0, "");
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
