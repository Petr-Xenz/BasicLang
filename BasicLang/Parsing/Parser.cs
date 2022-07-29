using BasicLang.AbstractTree;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang.Parsing
{
    internal class Parser
    {
        private readonly IReadOnlyList<Token> _tokens;

        private int _position;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.Where(t => t.Type != Comment).ToArray();
        }

        public ParseSyntaxTree Parse()
        {
            throw new NotImplementedException();
        }
    }
}
