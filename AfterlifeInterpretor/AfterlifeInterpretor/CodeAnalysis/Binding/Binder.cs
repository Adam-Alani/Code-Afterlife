using System;
using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Parser;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    /// <summary>
    /// Binder class
    /// The Binder an all related classes serve as a kind of parser that will also check types
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal sealed class Binder
    {
        private BoundScope _scope;

        public readonly Errors Errs;

        private bool _allowOverwriting = false;

        public Binder()
        {
            _scope = new BoundScope();
            Errs = new Errors();
        }
        
        public Binder(Scope scope)
        {
            _scope = new BoundScope(variables:scope.GetTypes(), functions:scope.GetFunctions());
            Errs = new Errors();
        }
        
        public Binder(Scope scope, Errors errs)
        {
            _scope = new BoundScope(variables:scope.GetTypes(), functions:scope.GetFunctions());
            Errs = errs;
        }

        public BoundBlockStatement BindProgram(BlockStatement program)
        {
            List<BoundStatement> statements = new List<BoundStatement>();

            foreach (StatementSyntax statement in program.Statements)
            {
                statements.Add(BindStatement(statement));
            }
            
            return new BoundBlockStatement(statements.ToArray(), program.Token.Position);
        }

        private BoundStatement BindStatement(StatementSyntax syntax)
        {
            switch (syntax?.Kind)
            {
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement((BlockStatement) syntax);
                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement((ExpressionStatement) syntax);
                case SyntaxKind.IfStatement:
                    return BindIfStatement((IfStatement) syntax);
                case SyntaxKind.WhileStatement:
                    return BindWhileStatement((WhileStatement) syntax);
                case SyntaxKind.ForStatement:
                    return BindForStatement((ForStatement) syntax);
                case SyntaxKind.ReturnStatement:
                    return BindReturnStatement((ReturnStatement) syntax);
                default:
                    Errs.ReportUnknown(syntax?.Kind, 0);
                    return null;
            }
        }

        private BoundStatement BindReturnStatement(ReturnStatement syntax)
        {
            BoundExpression bs = BindExpression(syntax.Expression);
            Type t = _scope.BlockType;
            if (t != null && t != bs?.Type && bs?.Type != typeof(Unpredictable) && t != typeof(Unpredictable))
            {
                Errs.ReportType("Invalid return type", t, bs?.Type, syntax.Token.Position);
                return new BoundReturn(bs, syntax.Token.Position);
            }
            _scope.BlockType = bs?.Type;
            if (bs?.Type == typeof(Function) && bs is BoundCallExpression bce)
            {
                string typeString = bce.F?.GetTypeDepth(bce.Depth);
                if (_scope.TypeString == null)
                    _scope.TypeString = "(" + typeString + ")";
                else if (_scope.TypeString != typeString)
                {
                    Errs.Report($"Invalid return type: expected {_scope.TypeString}, got {typeString}", bs.Position);
                }
            }
            
            return new BoundReturn(bs, syntax.Token.Position);
        }

        private BoundExpression BindFunctionDeclaration(FunctionDeclaration syntax)
        {
            int position = syntax.Variable.Token.Position;
            _allowOverwriting = true;
            BoundVariable assignee = BindVariable(syntax.Variable);
            _scope.SetFunction(assignee.Name, typeof(Unpredictable));
            _scope = new BoundScope(_scope);
            BoundExpression args = BindExpression(syntax.Args);
            _allowOverwriting = false;
            BoundStatement body = BindStatement(syntax.Body);
            _scope = _scope.Parent;
            _scope.SetFunction(assignee.Name, body?.Type);
            return new BoundFunction(assignee, args, body, position);
        }

        private BoundStatement BindWhileStatement(WhileStatement syntax)
        {
            BoundExpressionStatement condition = (BoundExpressionStatement)BindExpressionStatement(syntax.Condition);
            if (condition.Expression.Type != typeof(bool) && condition.Expression.Type != typeof(Unpredictable))
            {
                Errs.ReportType(condition.Expression.Type, typeof(bool), syntax.Token.Position);
                return null;
            }
            return new BoundWhile(condition, BindStatement(syntax.Then), syntax.Token.Position);
        }
        
        private BoundStatement BindForStatement(ForStatement syntax)
        {
            _scope = new BoundScope(_scope);
            
            BoundExpressionStatement initialisation = (BoundExpressionStatement)BindExpressionStatement(syntax.Initialisation);
            
            BoundExpressionStatement condition = (BoundExpressionStatement)BindExpressionStatement(syntax.Condition);
            if (condition?.Expression?.Type != typeof(bool) && condition?.Expression?.Type != typeof(Unpredictable))
            {
                Errs.ReportType(condition?.Expression?.Type, typeof(bool), syntax.Token.Position);
                return null;
            }
            
            BoundExpressionStatement incrementation = (BoundExpressionStatement)BindExpressionStatement(syntax.Incrementation);


            BoundStatement then = BindStatement(syntax.Then);
            _scope = _scope.Parent;
            return new BoundFor(initialisation, condition, incrementation, then, syntax.Token.Position);
        }

        private BoundStatement BindIfStatement(IfStatement syntax)
        {
            BoundExpressionStatement condition = (BoundExpressionStatement)BindExpressionStatement(syntax.Condition);
            if (condition?.Expression?.Type != typeof(bool) && condition?.Expression?.Type != typeof(Unpredictable))
            {
                Errs.ReportType(condition?.Expression?.Type, typeof(bool), syntax.Token.Position);
                return null;
            }

            BoundStatement elseClause = (syntax.Else != null) ? BindStatement(syntax.Else.Then) : null;
            BoundStatement then = BindStatement(syntax.Then);
            if (elseClause != null && elseClause.Type != then?.Type && elseClause.Type != typeof(Unpredictable) && then?.Type != typeof(Unpredictable))
            {
                Errs.ReportType("Unmatching types in if", then?.Type, elseClause.Type, syntax.Token.Position);
                return null;
            }
            
            return new BoundIf(condition, then, elseClause, syntax.Token.Position);
        }

        private BoundStatement BindBlockStatement(BlockStatement syntax)
        {
            _scope = new BoundScope(_scope);
            
            List<BoundStatement> statements = new List<BoundStatement>();

            
            foreach (StatementSyntax statement in syntax.Statements)
            {
                BoundStatement bs = BindStatement(statement);
                statements.Add(bs);
            }

            Type t = _scope.BlockType;
            string typeString = _scope.TypeString;
            _scope = _scope.Parent;
            
            return new BoundBlockStatement(statements.ToArray(), syntax.Token.Position, t, typeString);
        }
        
        private BoundStatement BindExpressionStatement(ExpressionStatement syntax)
        {
            return new BoundExpressionStatement(BindExpression(syntax.Expression), syntax.Token.Position);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax?.Kind)
            {
                case SyntaxKind.BinaryExpression:
                    return BindBinary((BinaryExpression)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnary((UnaryExpression)syntax);
                case SyntaxKind.LiteralExpression:
                    return BindLiteral((LiteralExpression)syntax);
                case SyntaxKind.AssignementExpression:
                    return BindAssignment((AssignementExpression) syntax);
                case SyntaxKind.VariableExpression:
                    return BindVariable((VariableExpression) syntax); 
                case SyntaxKind.IdentifierToken:
                    return BindIdentifier((IdentifierExpression) syntax);
                case SyntaxKind.EmptyExpression:
                    return new BoundEmptyExpression(syntax.Token.Position);
                case SyntaxKind.EmptyListExpression:
                    return new BoundEmptyListExpression(syntax.Token.Position);
                case SyntaxKind.CallExpression:
                    return BindCall((CallExpression) syntax);
                case SyntaxKind.FunctionDeclaration:
                    return BindFunctionDeclaration((FunctionDeclaration) syntax);
                case SyntaxKind.IfExpression:
                    return BindIfExpression((IfExpression) syntax);
                default:
                    Errs.ReportUnknown(syntax?.Kind, 0);
                    return null;
            }
        }

        private BoundExpression BindIfExpression(IfExpression syntax)
        {
            BoundExpression condition = BindExpression(syntax.Condition.Expression);
            if (condition.Type != typeof(bool))
            {
                Errs.ReportType(condition.Type, typeof(bool), syntax.Token.Position);
                return null;
            }

            BoundExpression elseClause = BindExpression(syntax.Else);
            BoundExpression then = BindExpression(syntax.Then);
            if (elseClause?.Type != then?.Type && elseClause?.Type != typeof(Unpredictable) && then?.Type != typeof(Unpredictable))
            {
                Errs.ReportType("Unmatching types in if", then?.Type, elseClause?.Type, syntax.Token.Position);
                return null;
            }
            
            return new BoundIfExpression(condition, then, elseClause, syntax.Token.Position);
        }

        private BoundCallExpression BindCall(CallExpression syntax)
        {
            if (syntax.Called is VariableExpression bv && !_scope.HasVariable(bv.Name.Token.Text))
            {
                Errs.ReportUndefined(bv.Name.Token.Text, typeof(Function), bv.Token.Position);
                return null;
            }
            
            BoundExpression fe = BindExpression(syntax.Called);
            if (fe == null)
                return null;
            BoundExpression args = BindExpression(syntax.Args);
            
            if (fe.Type != typeof(Function) && fe.Type != typeof(Unpredictable))
            {
                Errs.ReportType(fe.Type, typeof(Function), syntax.Called.Token.Position);
                return null;
            }


            if (fe is BoundVariable fbv)
            {
                Function f = _scope.GetFunction(fbv.Name);
                if (f != null && f.Type != typeof(Unpredictable))
                    return new BoundCallExpression(fe, args, f.Type, syntax.Called.Token.Position, f);
                return new BoundCallExpression(fe, args, typeof(Unpredictable),syntax.Called.Token.Position);
            }

            if (fe is BoundCallExpression fce)
            {
                if (fce.F?.GetTypeDepth(fce.Depth + 1) == Text.PrettyType(fce.F?.Type))
                    return new BoundCallExpression(fe, args, fce.F?.Type,syntax.Called.Token.Position, fce.F, fce.Depth + 1);
                return new BoundCallExpression(fe, args, fce.Type, syntax.Called.Token.Position, fce.F, fce.Depth + 1);
            }

            return new BoundCallExpression(fe, args, typeof(Unpredictable),syntax.Called.Token.Position);
        }

        private BoundVariable BindIdentifier(IdentifierExpression syntax)
        {
            string name = syntax.Token.Text;
            if (!_scope.TryGetType(name, out Type t))
            {
                Errs.ReportUnknown(name, syntax.Token.Position);
                return null;
            }
            
            return new BoundVariable(name, t, syntax.Token.Position);
        }

        private BoundVariable BindVariable(VariableExpression syntax)
        {
            Type type = BoundVariableTypes.GetType(syntax.Token);
            
            if (type == null)
            {
                Errs.ReportUnknown(syntax.Token.Text, syntax.Token.Position);
                return new BoundVariable(syntax.Name.Token.Text, null, syntax.Token.Position);
            }
            
            if (!_scope.TryDeclare(syntax.Name.Token.Text, type) && !_allowOverwriting)
            {
                Errs.ReportDeclared(syntax.Name.Token.Text, syntax.Token.Position);
                return null;
            }

            return new BoundVariable(syntax.Name.Token.Text, type, syntax.Token.Position);
        }

        private BoundExpression BindAssignment(AssignementExpression syntax)
        {
            BoundExpression assignment = BindExpression(syntax.Assignment);

            if (assignment == null)
                return null;

            BoundExpression assignee = BindExpression(syntax.Assignee);
            if (assignee is BoundVariable bv)
                return BindAssignmentVariable(assignment, bv, syntax);
            if (syntax.Token.Kind == SyntaxKind.AssignToken && IsUnpacking(assignee, assignment))
                return new BoundAssignmentUnpacking((BoundBinary)assignee, assignment, syntax.Token.Position);

            Errs.ReportUnexpected(typeof(BoundVariable), assignee, syntax.Token.Position);
            return null;
        }

        private bool IsUnpacking(BoundExpression assignee, BoundExpression assignment)
        {
            return (assignee is BoundBinary be && be.Operator.Kind == BoundBinaryKind.Comma && !(be.Left is BoundEmptyListExpression) &&
                    (assignment is BoundBinary abe && abe.Operator.Kind == BoundBinaryKind.Comma || 
                     assignment is BoundVariable bv && _scope.TryGetType(bv.Name, out Type t) && t == typeof(List)) 
                    && IsDeclarationList(be));
        }

        private bool IsDeclarationList(BoundBinary be)
        {
            return (be.Left is BoundVariable || (be.Left is BoundBinary be1 && IsDeclarationList(be1)) &&
                (be.Right is BoundEmptyExpression || be.Right is BoundVariable ||
                 be.Right is BoundBinary be2 && IsDeclarationList(be2)));
        }

        private BoundAssignment BindAssignmentVariable(BoundExpression assignement, BoundVariable bv, AssignementExpression syntax)
        {
            if (bv.Type == null)
            {
                Errs.ReportUnknown(bv.Name, syntax.Token.Position);
                return null;
            }

            if (assignement.Type == typeof(object) || assignement.Type == typeof(Unpredictable))
            {
                return new BoundAssignment(bv, assignement, syntax.Token.Kind, syntax.Token.Position);
            }
                
            if (bv.Type != assignement.Type && bv.Type != typeof(object) && BoundBinaryOperator.Bind(syntax.Token.Kind, bv.Type, assignement.Type) == null)
            {
                Errs.ReportType(syntax.Token.Text, bv.Type, assignement.Type, syntax.Token.Position);
                return null;
            }
                
            if (bv.Type == typeof(object))
                _scope.ChangeType(bv.Name, assignement.Type);
            return new BoundAssignment(bv, assignement, syntax.Token.Kind, syntax.Token.Position);
        }
        

        private BoundUnary BindUnary(UnaryExpression syntax)
        {
            BoundExpression operandBound = BindExpression(syntax.Operand);
            BoundUnaryOperator uOperator = BoundUnaryOperator.Bind(syntax.Token.Kind, operandBound.Type);
            if (uOperator == null)
            {
                Errs.ReportUndefined(syntax.Token.Text, operandBound.Type, syntax.Token.Position);
                return null;
            }
            return new BoundUnary(uOperator, operandBound, syntax.Token.Position);
        }

        private BoundLiteral BindLiteral(LiteralExpression syntax)
        {
            return new BoundLiteral(syntax.Value ?? 0, syntax.Token.Position);
        }

        private BoundBinary BindBinary(BinaryExpression syntax)
        {
            BoundExpression left = BindExpression(syntax.Left);
            BoundExpression right = BindExpression(syntax.Right);

            if (left == null || right == null)
                return null;

            Type leftType = left.Type;
            Type rightType = right.Type;

            BoundBinaryOperator bOperator = BoundBinaryOperator.Bind(syntax.Token.Kind, leftType, rightType);

            if (bOperator == null)
            {
                BoundBinaryOperator.HarmoniseTypes(ref leftType, ref rightType);
                bOperator = BoundBinaryOperator.Bind(syntax.Token.Kind, leftType, rightType);
            }

            if (bOperator == null)
            {
                Errs.ReportType(syntax.Token.Text, left.Type, right.Type, syntax.Token.Position);
                return null;
            }
            
            return new BoundBinary(left, bOperator, right, syntax.Token.Position);
        }
    }
}