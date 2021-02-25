using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
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
                tk = lex.Lex();
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

        public SyntaxTree Parse()
        {
            return new SyntaxTree(Errors, ParseAssignements(), Expect(SyntaxKind.EndToken));
        }

        private ExpressionSyntax ParseAssignements()
        {
            ExpressionSyntax assignee = ParseExpression();

            while (Current.Kind == SyntaxKind.AssignToken)
            {
                SyntaxToken op = NextToken();
                ExpressionSyntax assignment = ParseAssignements();
                
                assignee = new AssignementExpression(assignee, op, assignment);
            }

            return assignee;
        }
        
        private ExpressionSyntax PrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OParenToken:
                    SyntaxToken open = NextToken();
                    ExpressionSyntax expr = ParseExpression();
                    SyntaxToken close = Expect(SyntaxKind.CParenToken);

                    return expr;
                    // return new ParenthesisedExpression(open, expr, close);
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    bool trueFalse = Current.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpression(NextToken(), trueFalse);
                case SyntaxKind.VarToken:
                case SyntaxKind.BoolToken:
                case SyntaxKind.IntToken:
                    return new VariableExpression(NextToken(), new IdentifierExpression(Expect(SyntaxKind.IdentifierToken)));
                case SyntaxKind.IdentifierToken:
                    return new IdentifierExpression(NextToken());
                default:
                    return new LiteralExpression(Expect(SyntaxKind.NumericToken));
            }
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            int unaryPrecedence = Current.Kind.GetUnaryPrecedence();
            if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedence)
            {
                left = new UnaryExpression(NextToken(), ParseExpression(unaryPrecedence));
            }
            else
            {
                left = PrimaryExpression();
            }
            
            bool doOperation;
            do
            {
                doOperation = false;
                int precedence = Current.Kind.GetBinaryPrecedence();
                if (precedence != 0 && precedence > parentPrecedence)
                {
                    doOperation = true;
                    SyntaxToken operatorToken = NextToken();
                    ExpressionSyntax right = ParseExpression(precedence);
                    left = new BinaryExpression(left, operatorToken, right);
                }
            } while (doOperation);
            
            return left;
        }
    }
}