using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLang.AbstractTree
{
    internal class Token : ISourceLocation
    {
        public Token(TokenType type, int line, int column, int length, string value, WhiteSpaceTrivia trivia = default)
        {
            Type = type;
            Line = line;
            Column = column;
            Length = length;
            Value = value;
            WhiteSpaceTrivia = trivia;
        }

        public TokenType Type { get; }
        
        public int Line { get; }

        public int Column { get; }

        public int Length { get; }

        public string Value { get; }

        public WhiteSpaceTrivia WhiteSpaceTrivia { get; }
    }
}
