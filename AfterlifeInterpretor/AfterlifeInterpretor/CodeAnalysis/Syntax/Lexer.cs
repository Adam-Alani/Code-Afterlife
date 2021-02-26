using System;
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

        private SyntaxToken GetToken()
        {
            switch (Current)
            {
                case '\0':
                    return new SyntaxToken(SyntaxKind.EndToken, _position, "\0");
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+");
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-");
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*");
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/");
                case '%':
                    return new SyntaxToken(SyntaxKind.ModuloToken, _position++, "%");
                case '(':
                    return new SyntaxToken(SyntaxKind.OParenToken, _position++, "(");
                case ')':
                    return new SyntaxToken(SyntaxKind.CParenToken, _position++, ")");
                case '&':
                    if (LookAhead == '&')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AndToken, _position - 2, "&&");
                    }
                    break;
                case '|':
                    if (LookAhead == '|')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.OrToken, _position - 2, "||");
                    }
                    break;
                case '!':
                    if (LookAhead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.NEqToken, _position - 2, "!=");
                    }
                    return new SyntaxToken(SyntaxKind.NotToken, _position++, "!");
                case '=':
                    if (LookAhead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.EqToken, _position - 2, "==");
                    }
                    return new SyntaxToken(SyntaxKind.AssignToken, _position++, "=");
                case '>':
                    if (LookAhead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.GtEqToken, _position - 2, ">=");
                    }
                    return new SyntaxToken(SyntaxKind.GtToken, _position ++, ">");
                case '<':
                    if (LookAhead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.LtEqToken, _position - 2, "<=");
                    }
                    return new SyntaxToken(SyntaxKind.LtToken, _position ++, "<");
            }
            
            return new SyntaxToken(SyntaxKind.ErrorToken, _position++, $"{_text[_position - 1]}");
        }

        public SyntaxToken Lex()
        {
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

            SyntaxToken token = GetToken();
            if (token.Kind == SyntaxKind.ErrorToken)
                Errs.ReportUnknown(token.Text, _position);
            return token;
        }

        private SyntaxToken LexKeyword()
        {
            string text = "";

            while (char.IsLetter(Current))
            {
                text += Current;
                _position++;
            }

            return new SyntaxToken(SyntaxFacts.GetKeywordKind(text), _position, text);
        }

        private SyntaxToken LexWhiteSpace()
        {
            string text = "";

            while (char.IsWhiteSpace(Current))
            {
                text += Current;
                _position++;
            }

            return new SyntaxToken(SyntaxKind.SpaceToken, _position, text);
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

            int value;
            if (int.TryParse(text, out value))
                return new SyntaxToken(SyntaxKind.NumericToken, start, text, value);

            Errs.ReportUnexpected(SyntaxKind.NumericToken, text, _position);
            return new SyntaxToken(SyntaxKind.ErrorToken, start, text);
        }
    }
}