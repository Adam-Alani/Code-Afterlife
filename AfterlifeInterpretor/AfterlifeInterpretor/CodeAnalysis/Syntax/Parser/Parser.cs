using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
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

        public Errors Errs;

        public Parser(string text)
        {
            Lexer.Lexer lex = new Lexer.Lexer(text);
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
            
            Errs = lex.Errs;
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
            
            Errs.ReportUnexpected(kind, Current.Kind, Current.Position);
            _position++;
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
            return new SyntaxTree(Errs, ParseProgram(), Expect(SyntaxKind.EndToken));
        }
        
        private void SkipEndStatements()
        {
            while (Current.Kind == SyntaxKind.EndStatementToken)
                NextToken();
        }

        private bool ExpectEnd(SyntaxNode start)
        {
            return start.Kind != SyntaxKind.IfKeyword &&
                   start.Kind != SyntaxKind.WhileKeyword &&
                   Current.Kind != SyntaxKind.EndToken &&
                   Current.Kind != SyntaxKind.OBlockToken &&
                   Current.Kind != SyntaxKind.CBlockToken;
        }
        
        private BlockStatement ParseProgram()
        {
            SyntaxToken oToken = new SyntaxToken(SyntaxKind.OBlockToken, _position, "");
            StatementSyntax[] statements = ParseStatements();
            SyntaxToken cToken = new SyntaxToken(SyntaxKind.CBlockToken, _position, "");
            
            return new BlockStatement(oToken, statements, cToken);
        }
        
        private StatementSyntax[] ParseStatements()
        {
            List<StatementSyntax> statements = new List<StatementSyntax>();
            
            while (Current.Kind != SyntaxKind.EndToken && Current.Kind != SyntaxKind.CBlockToken)
            {
                SyntaxToken start = Current;
                if (Current.Kind != SyntaxKind.EndStatementToken)
                    statements.Add(ParseStatement());
                if (Current == start)
                    NextToken();
                else if (ExpectEnd(start)) 
                    Expect(SyntaxKind.EndStatementToken);
            }

            return statements.ToArray();
        }

        private StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OBlockToken:
                    return ParseBlockStatement();
                case SyntaxKind.IfKeyword:
                    return ParseIfStatement();
                case SyntaxKind.WhileKeyword:
                    return ParseWhileStatement();
                case SyntaxKind.ForKeyword:
                    return ParseForStatement();
                default:
                    return ParseExpressionStatement();
            }
        }

        private StatementSyntax ParseBlockStatement()
        {
            SyntaxToken oToken = Expect(SyntaxKind.OBlockToken);
            StatementSyntax[] statements = ParseStatements();
            SyntaxToken cToken = Expect(SyntaxKind.CBlockToken);
            
            return new BlockStatement(oToken, statements, cToken);
        }
        
        private StatementSyntax ParseExpressionStatement()
        {
            return new ExpressionStatement(ParseAssignments());
        }

        private StatementSyntax ParseWhileStatement()
        {
            SyntaxToken token = Expect(SyntaxKind.WhileKeyword);
            ExpressionStatement condition = (ExpressionStatement) ParseExpressionStatement();
            SkipEndStatements();
            StatementSyntax then = ParseStatement();
            SkipEndStatements();
            return new WhileStatement(token, condition, then);
        }
        
        private StatementSyntax ParseForStatement()
        {
            SyntaxToken token = Expect(SyntaxKind.ForKeyword);
            bool mustClose = false;
            if (Current.Kind == SyntaxKind.OParenToken)
            {
                mustClose = true;
                NextToken();
            }
                
            ExpressionStatement initialisation = (ExpressionStatement) ParseExpressionStatement();
            Expect(SyntaxKind.EndStatementToken);
            ExpressionStatement condition = (ExpressionStatement) ParseExpressionStatement();
            Expect(SyntaxKind.EndStatementToken);
            ExpressionStatement incrementation = (Current.Kind == SyntaxKind.OBlockToken) 
                ? new ExpressionStatement(new EmptyExpression()) 
                : (ExpressionStatement) ParseExpressionStatement();
            SkipEndStatements();
            if (mustClose)
                Expect(SyntaxKind.CParenToken);
            SkipEndStatements();
            StatementSyntax then = ParseStatement();
            SkipEndStatements();
            return new ForStatement(token, initialisation, condition, incrementation, then);
        }

        private StatementSyntax ParseIfStatement()
        {
            SyntaxToken token = Expect(SyntaxKind.IfKeyword);
            ExpressionStatement condition = (ExpressionStatement)ParseExpressionStatement();
            SkipEndStatements();
            StatementSyntax then = ParseStatement();
            SkipEndStatements();
            return new IfStatement(token, condition, then, ParseElseClause());
        }

        private ElseClause ParseElseClause()
        {
            if (Current.Kind != SyntaxKind.ElseKeyword)
                return null;
            
            SyntaxToken token = NextToken();
            SkipEndStatements();
            StatementSyntax statement = ParseStatement();
            return new ElseClause(token, statement);
        }

        private ExpressionSyntax ParseAssignments()
        {
            ExpressionSyntax assignee = ParseExpression();

            while (SyntaxFacts.IsAssignment(Current.Kind))
            {
                SyntaxToken op = NextToken();
                ExpressionSyntax assignment = ParseAssignments();
                
                assignee = new AssignementExpression(assignee, op, assignment);
            }

            return assignee;
        }
        
        private ExpressionSyntax PrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OParenToken:
                    NextToken();
                    SkipEndStatements();
                    ExpressionSyntax expr = ParseAssignments(); 
                    SkipEndStatements();
                    Expect(SyntaxKind.CParenToken);

                    return expr;
                    // return new ParenthesisedExpression(open, expr, close);
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    bool trueFalse = Current.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpression(NextToken(), trueFalse);
                case SyntaxKind.ListToken:
                case SyntaxKind.StringToken:
                case SyntaxKind.FloatToken:
                case SyntaxKind.VarToken:
                case SyntaxKind.BoolToken:
                case SyntaxKind.IntToken:
                    return new VariableExpression(NextToken(), new IdentifierExpression(Expect(SyntaxKind.IdentifierToken)));
                case SyntaxKind.IdentifierToken:
                    return new IdentifierExpression(NextToken());
                case SyntaxKind.CBlockToken:
                case SyntaxKind.CParenToken:
                case SyntaxKind.EndToken:
                case SyntaxKind.EndStatementToken:
                    return new EmptyExpression();
                case SyntaxKind.WordToken:
                    return new LiteralExpression(Expect(SyntaxKind.WordToken));
                case SyntaxKind.CommaToken:
                    Expect(SyntaxKind.CommaToken);
                    return new EmptyListExpression();
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
                if (precedence != 0 && precedence > parentPrecedence || (precedence == parentPrecedence && !Current.Kind.IsRightAssociative()))
                {
                    doOperation = true;
                    SyntaxToken operatorToken = NextToken();
                    ExpressionSyntax right = (Current.Kind == SyntaxKind.EndStatementToken) ? new EmptyExpression() : ParseExpression(precedence);
                    left = new BinaryExpression(left, operatorToken, right);
                }
            } while (doOperation);
            
            return left;
        }
    }
}