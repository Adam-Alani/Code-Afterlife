namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    /// <summary>
    /// Lexer class
    /// Tokenise a given code
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal class Lexer
    {
        private readonly string _text;
        private int _position;

        private char Current => Peek(0);

        private char LookAhead => Peek(1);

        private char Peek(int offset)
        {
            if (_position + offset >= _text.Length)
                return '\0';
            return _text[_position + offset];
        }

        public Errors Errs;

        public Lexer(string text, Errors errs)
        {
            _text = text;
            
            Errs = errs;
        }
        
        public Lexer(string text)
        {
            _text = text;
            
            Errs = new Errors();
        }

        private SyntaxKind FindKind(string s)
        {
            return s switch
            {
                "\0" => SyntaxKind.EndToken,
                "+" => SyntaxKind.PlusToken,
                "-" => SyntaxKind.MinusToken,
                "/" => SyntaxKind.SlashToken,
                "%" => SyntaxKind.ModuloToken,
                "(" => SyntaxKind.OParenToken,
                ")" => SyntaxKind.CParenToken,
                ">" => SyntaxKind.GtToken,
                "<" => SyntaxKind.LtToken,
                "=" => SyntaxKind.AssignToken,
                "!" => SyntaxKind.NotToken,
                "||" => SyntaxKind.OrToken,
                "&&" => SyntaxKind.AndToken,
                "==" => SyntaxKind.EqToken,
                "!=" => SyntaxKind.NEqToken,
                ">=" => SyntaxKind.GtEqToken,
                "<=" => SyntaxKind.LtEqToken,
                _ => SyntaxKind.ErrorToken
            };
        }

        private bool ExpectedLookahead()
        {
            switch (Current)
            {
                case '\0':
                    return false;
                case '!':
                case '>':
                case '<':
                    return LookAhead == '=';
                default:
                    return LookAhead == Current;
            }
        }

        private SyntaxToken FindToken()
        {
            int start = _position;
            string text = $"{Current}";
            if (ExpectedLookahead())
            {
                text += LookAhead;
                _position++;
            }
            _position++;


            return new SyntaxToken(FindKind(text), start, text);
        }

        public SyntaxToken Lex()
        {
            int start = _position;
            if (char.IsDigit(Current))
            {
                return LexDigit();
            }

            if (char.IsWhiteSpace(Current))
            {
                return LexWhiteSpace();
            }

            if (char.IsLetter(Current))
            {
                return LexKeyword();
            }

            SyntaxToken token = FindToken();
            if (token.Kind == SyntaxKind.ErrorToken)
                Errs.ReportUnknown(token.Text, start);
            return token;
        }

        private SyntaxToken LexKeyword()
        {
            int start = _position;
            string text = "";

            while (char.IsLetter(Current))
            {
                text += Current;
                _position++;
            }

            return new SyntaxToken(SyntaxFacts.GetKeywordKind(text), start, text);
        }

        private SyntaxToken LexWhiteSpace()
        {
            int start = _position;
            string text = "";

            while (char.IsWhiteSpace(Current))
            {
                text += Current;
                _position++;
            }

            return new SyntaxToken(SyntaxKind.SpaceToken, start, text);
        }

        private SyntaxToken LexDigit()
        {
            int start = _position;
            string text = "";

            while (char.IsDigit(Current))
            {
                text += Current;
                _position++;
            }

            if (int.TryParse(text, out var value))
                return new SyntaxToken(SyntaxKind.NumericToken, start, text, value);

            Errs.ReportUnexpected(SyntaxKind.NumericToken, text, start);
            return new SyntaxToken(SyntaxKind.ErrorToken, start, text);
        }
    }
}