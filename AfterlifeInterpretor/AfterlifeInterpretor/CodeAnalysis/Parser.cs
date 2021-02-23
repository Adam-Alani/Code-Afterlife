using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis
{
    /// <summary>
    /// Parser class
    /// Parses given code in order to create a syntax tree
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;

        private SyntaxToken Current => Peek(0);
        
        public List<string> Errors { get; }

        public Parser(string text)
        {
            Lexer lex = new Lexer(text);
            List<SyntaxToken> tokens = new List<SyntaxToken>();

            SyntaxToken tk;
            do
            {
                tk = lex.NextToken();
                if (tk.Kind != SyntaxKind.SpaceToken && tk.Kind != SyntaxKind.ErrorToken)
                    tokens.Add(tk);
            } while (tk.Kind != SyntaxKind.EndToken);

            _tokens = tokens.ToArray();
            _position = 0;
            
            Errors = lex.Errors;
        }

        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[^1];
            return _tokens[index];
        }

        private SyntaxToken Expect(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();
            
            Errors.Add($"ERROR: Unexpected token {Current.Text} at {Current.Position}\n" +
                            $"Token is {Current.Kind}, expected {kind}");
            return new SyntaxToken(kind, Current.Position, null);
        }

        private SyntaxToken NextToken()
        {
            SyntaxToken cur = Current;
            _position++;
            return cur;
        }

        private ExpressionSyntax PrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OParenToken)
            {
                SyntaxToken open = NextToken();
                ExpressionSyntax expr = Term();
                SyntaxToken close = Expect(SyntaxKind.CParenToken);

                return new ParenthesisedExpression(open, expr, close);
            }
            return new LiteralExpression(Expect(SyntaxKind.NumericToken));
        }

        private ExpressionSyntax Factor()
        {
            ExpressionSyntax left = PrimaryExpression();

            while (Current.Kind == SyntaxKind.StarToken || 
                   Current.Kind == SyntaxKind.SlashToken || 
                   Current.Kind == SyntaxKind.ModuloToken)
            {
                left = new BinaryExpression(left, NextToken(), PrimaryExpression());
            }

            return left;
        }

        private ExpressionSyntax Term()
        {
            ExpressionSyntax left = Factor();

            while (Current.Kind == SyntaxKind.PlusToken || 
                   Current.Kind == SyntaxKind.MinusToken)
            {
                left = new BinaryExpression(left, NextToken(), Factor());
            }

            return left;
        }
        
        private ExpressionSyntax Expression()
        {
            ExpressionSyntax left = Term();

            while (Current.Kind == SyntaxKind.PlusToken || Current.Kind == SyntaxKind.MinusToken)
            {
                left = new BinaryExpression(left, NextToken(), Term());
            }

            return left;
        }

        public SyntaxTree Parse()
        {
            return new SyntaxTree(Errors, Expression(), Expect(SyntaxKind.EndToken));
        }
    }
}