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
                return _tokens[_tokens.Length - 1];
            return _tokens[index];
        }

        private SyntaxToken Expect(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();
            
            Errs.ReportUnexpected(kind, Current.Kind, Current.Position);
            if (Peek(1).Kind != SyntaxKind.EndToken)
                _position++;
            return new SyntaxToken(kind, Current.Position, Current.ToString());
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
                   start.Kind != SyntaxKind.ForKeyword &&
                   Current.Kind != SyntaxKind.EndToken &&
                   Current.Kind != SyntaxKind.OBlockToken &&
                   Current.Kind != SyntaxKind.CBlockToken;
        }

        private bool IsValueToken(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.OParenToken:
                case SyntaxKind.NumericToken:
                case SyntaxKind.WordToken:
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                case SyntaxKind.IdentifierToken:
                    return true;
                default:
                    return false;
            }
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
                case SyntaxKind.WhileKeyword:
                    return ParseWhileStatement();
                case SyntaxKind.ForKeyword:
                    return ParseForStatement();
                case SyntaxKind.IfKeyword:
                    return ParseIfStatement();
                case SyntaxKind.ReturnKeyword:
                    return new ReturnStatement(Expect(SyntaxKind.ReturnKeyword), ParseExpression());
                default:
                    return ParseExpressionStatement();
            }
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
                ? new ExpressionStatement(new EmptyExpression(Current)) 
                : (ExpressionStatement) ParseExpressionStatement();
            SkipEndStatements();
            if (mustClose)
                Expect(SyntaxKind.CParenToken);
            SkipEndStatements();
            StatementSyntax then = ParseStatement();
            SkipEndStatements();
            return new ForStatement(token, initialisation, condition, incrementation, then);
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
                    
                    if (expr is CallExpression ce && IsValueToken(Current.Kind))
                        return new CallExpression(ce, ParseArguments(true));
                    
                    return expr;
                    // return new ParenthesisedExpression(open, expr, close);
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    bool trueFalse = Current.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpression(NextToken(), trueFalse);
                case SyntaxKind.ListKeyword:
                case SyntaxKind.StringKeyword:
                case SyntaxKind.FloatKeyword:
                case SyntaxKind.VarKeyword:
                case SyntaxKind.BoolKeyword:
                case SyntaxKind.IntKeyword:
                    return new VariableExpression(NextToken(), new IdentifierExpression(Expect(SyntaxKind.IdentifierToken)));
                case SyntaxKind.IdentifierToken:
                    if (IsValueToken(Peek(1).Kind))
                        return new CallExpression(new IdentifierExpression(NextToken()), ParseArguments(true, true));
                    return new IdentifierExpression(NextToken());
                case SyntaxKind.FunctionKeyword:
                    return ParseFunctionDeclaration();
                case SyntaxKind.IfKeyword:
                    return ParseIfExpression();
                case SyntaxKind.CBlockToken:
                case SyntaxKind.CParenToken:
                case SyntaxKind.EndToken:
                case SyntaxKind.EndStatementToken:
                    return new EmptyExpression(Current);
                case SyntaxKind.WordToken:
                    return new LiteralExpression(Expect(SyntaxKind.WordToken));
                case SyntaxKind.CommaToken:
                    return new EmptyListExpression(Expect(SyntaxKind.CommaToken));
                default:
                    return new LiteralExpression(Expect(SyntaxKind.NumericToken));
            }
        }

        private ExpressionSyntax ParseIfExpression()
        {
            SyntaxToken token = Expect(SyntaxKind.IfKeyword);
            ExpressionStatement condition = (ExpressionStatement)ParseExpressionStatement();
            SkipEndStatements();
            ExpressionSyntax then = ParseExpression();
            SkipEndStatements();
            Expect(SyntaxKind.ElseKeyword);
            return new IfExpression(token, condition, then, ParseAssignments());
        }

        private ExpressionSyntax ParseFunctionDeclaration()
        {
            SyntaxToken token = Expect(SyntaxKind.FunctionKeyword);
            SyntaxToken name = Expect(SyntaxKind.IdentifierToken);


            ExpressionSyntax args = (Current.Kind == SyntaxKind.AssignToken)
                ? new EmptyListExpression(Current)
                : ParseArguments();
            if (!(args is BinaryExpression) && !(args is EmptyListExpression))
            {
                Errs.ReportUnexpected("List", args.Kind,  token.Position);
                return new FunctionDeclaration(token, new IdentifierExpression(name), new EmptyExpression(args.Token), null);
            }

            // Possible improvement: adding sub functions, I didn't manage to do it because of scoping issue,
            // and I'm too tired to find a solution
            // considering that it is not important, it is fine
            /*Stack<ExpressionSyntax> subArgs = new Stack<ExpressionSyntax>();
            while (Current.Kind != SyntaxKind.AssignToken && Current.Kind != SyntaxKind.EndToken)
            {
                subArgs.Push(ParseArguments());
            } */
            
            Expect(SyntaxKind.AssignToken);
            SkipEndStatements();
            StatementSyntax body = (Current.Kind == SyntaxKind.OBlockToken) ? ParseStatement() : new ExpressionStatement(ParseExpression());
            
            /*while (subArgs.Count > 0)
            {
                ExpressionSyntax subArg = subArgs.Pop();
                SyntaxToken subName = new SyntaxToken(SyntaxKind.IdentifierToken, subArg.Token.Position, "_");
                body = new ExpressionStatement(new FunctionDeclaration(token, new IdentifierExpression(subName), subArg, body));
            }*/

            return new FunctionDeclaration(token, new IdentifierExpression(name), args, body);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0, bool stopAtNotComma = false)
        {
            ExpressionSyntax left = ParseUnary(parentPrecedence);
            while (left is CallExpression ce && IsValueToken(Current.Kind))
                left = new CallExpression(ce, ParseArguments(true));

            if (stopAtNotComma && Current.Kind != SyntaxKind.CommaToken)
                return left;

            bool doOperation;
            do
            {
                doOperation = false;
                int precedence = Current.Kind.GetBinaryPrecedence();
                if (precedence != 0 && precedence > parentPrecedence || (precedence == parentPrecedence && !Current.Kind.IsRightAssociative()))
                {
                    doOperation = true;
                    SyntaxToken operatorToken = NextToken();
                    ExpressionSyntax right = (Current.Kind == SyntaxKind.EndStatementToken) ? new EmptyExpression(Current) : ParseExpression(precedence);
                    left = new BinaryExpression(left, operatorToken, right);
                }
            } while (doOperation);
            
            return left;
        }

        private ExpressionSyntax ParseUnary(int parentPrecedence)
        {
            int unaryPrecedence = Current.Kind.GetUnaryPrecedence();
            if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedence)
            {
                return new UnaryExpression(NextToken(), ParseExpression(unaryPrecedence));
            }
            return PrimaryExpression();
        }

        private ExpressionSyntax ParseArguments(bool allowNonvariable = false, bool stopAtNotComma = false)
        {
            ExpressionSyntax args = ParseExpression(stopAtNotComma: stopAtNotComma);
            if (allowNonvariable && !(args.Token.Kind == SyntaxKind.CommaToken || args is EmptyExpression))
                args = new BinaryExpression(args, new SyntaxToken(SyntaxKind.CommaToken, args.Token.Position, ""),
                    new EmptyExpression(args.Token));
            else if (args is VariableExpression ve)
                args = new BinaryExpression(ve, new SyntaxToken(SyntaxKind.CommaToken, ve.Token.Position, ""),
                    new EmptyExpression(args.Token));
            if (args is EmptyExpression)
                args = new EmptyListExpression(args.Token);
            return args;
        }
    }
}