using System.Collections.Generic;

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

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
        }

        public List<string> Errors { get; }

        public Lexer(string text)
        {
            _text = text;
            
            Errors = new List<string>();
        }

        private SyntaxToken GetToken()
        {
            return Current switch
            {
                '\0' => new SyntaxToken(SyntaxKind.EndToken,    _position,   "\0"                     ),
                '+'  => new SyntaxToken(SyntaxKind.PlusToken,   _position++, "+"                      ),
                '-'  => new SyntaxToken(SyntaxKind.MinusToken,  _position++, "-"                      ),
                '*'  => new SyntaxToken(SyntaxKind.StarToken,   _position++, "*"                      ),
                '/'  => new SyntaxToken(SyntaxKind.SlashToken,  _position++, "/"                      ),
                '%'  => new SyntaxToken(SyntaxKind.ModuloToken, _position++, "%"                      ),
                '('  => new SyntaxToken(SyntaxKind.OParenToken, _position++, "("                      ),
                ')'  => new SyntaxToken(SyntaxKind.CParenToken, _position++, ")"                      ),
                _    => new SyntaxToken(SyntaxKind.ErrorToken,  _position++, $"{_text[_position - 1]}")
            };
        }

        public SyntaxToken Lex()
        {
            if (char.IsDigit(Current))
            {
                int start = _position;
                string text = "";

                while (char.IsDigit(Current))
                {
                    text += Current;
                    _position++;
                }

                int value;
                if (int.TryParse(text, out value))
                    return new SyntaxToken(SyntaxKind.NumericToken, start, text, value);
                
                Errors.Add($"ERROR: Expected a number in {text} at {_position}\n" +
                                $"Expected {SyntaxKind.NumericToken}");
                return new SyntaxToken(SyntaxKind.ErrorToken, start, text);
            }

            if (char.IsWhiteSpace(Current))
            {
                int start = _position;
                string text = "";

                while (char.IsWhiteSpace(Current))
                {
                    text += Current;
                    _position++;
                }
                
                return new SyntaxToken(SyntaxKind.SpaceToken, _position, text);
            }

            SyntaxToken token = GetToken();
            if (token.Kind == SyntaxKind.ErrorToken)
                Errors.Add($"ERROR: Unknown token {token.Text} at {token.Position}");
            return token;
        }
    }
}