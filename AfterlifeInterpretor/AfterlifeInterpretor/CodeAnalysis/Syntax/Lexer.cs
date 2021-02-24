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

        public List<string> Errors { get; }

        public Lexer(string text)
        {
            _text = text;
            
            Errors = new List<string>();
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

                    break;
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
                default:
                    return new SyntaxToken(SyntaxKind.ErrorToken, _position++, $"{_text[_position - 1]}");
            }
            return new SyntaxToken(SyntaxKind.ErrorToken, _position++, $"{_text[_position - 1]}");
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

            if (char.IsLetter(Current))
            {
                int start = _position;
                string text = "";

                while (char.IsLetter(Current))
                {
                    text += Current;
                    _position++;
                }
                
                return new SyntaxToken(SyntaxFacts.GetKeywordKind(text), _position, text);
            }

            SyntaxToken token = GetToken();
            if (token.Kind == SyntaxKind.ErrorToken)
                Errors.Add($"ERROR: Unknown token {token.Text} at {token.Position}");
            return token;
        }
    }
}