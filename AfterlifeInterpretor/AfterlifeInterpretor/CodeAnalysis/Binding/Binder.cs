using System;
using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax;
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

        public Binder()
        {
            _scope = new BoundScope();
            Errs = new Errors();
        }
        
        public Binder(Scope scope)
        {
            _scope = new BoundScope(scope.GetTypes());
            Errs = new Errors();
        }
        
        public Binder(Scope scope, Errors errs)
        {
            _scope = new BoundScope(scope.GetTypes());
            Errs = errs;
        }

        public BoundBlockStatement BindProgram(BlockStatement program)
        {
            List<BoundStatement> statements = new List<BoundStatement>();

            foreach (StatementSyntax statement in program.Statements)
            {
                statements.Add(BindStatement(statement));
            }
            
            return new BoundBlockStatement(statements.ToArray(), program.OpenToken.Position);
        }

        private BoundStatement BindStatement(StatementSyntax syntax)
        {
            switch (syntax.Kind)
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
                default:
                    Errs.ReportUnknown(syntax.Kind, 0);
                    return null;
            }
        }

        private BoundStatement BindWhileStatement(WhileStatement syntax)
        {
            BoundExpressionStatement condition = (BoundExpressionStatement)BindExpressionStatement(syntax.Condition);
            if (condition.Expression.Type != typeof(bool))
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
            if (condition?.Expression?.Type != typeof(bool))
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
            if (condition.Expression.Type != typeof(bool))
            {
                Errs.ReportType(condition.Expression.Type, typeof(bool), syntax.Token.Position);
                return null;
            }

            BoundStatement elseClause = (syntax.Else != null) ? BindStatement(syntax.Else.Then) : null;
            return new BoundIf(condition, BindStatement(syntax.Then), elseClause, syntax.Token.Position);
        }

        private BoundStatement BindBlockStatement(BlockStatement syntax)
        {
            _scope = new BoundScope(_scope);
            
            List<BoundStatement> statements = new List<BoundStatement>();

            foreach (StatementSyntax statement in syntax.Statements)
            {
                statements.Add(BindStatement(statement));
            }

            _scope = _scope.Parent;
            
            return new BoundBlockStatement(statements.ToArray(), syntax.OpenToken.Position);
        }
        
        private BoundStatement BindExpressionStatement(ExpressionStatement syntax)
        {
            return new BoundExpressionStatement(BindExpression(syntax.Expression), -1);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.BinaryExpression:
                    return BindBinary((BinaryExpression)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnary((UnaryExpression)syntax);
                case SyntaxKind.LiteralExpression:
                    return BindLiteral((LiteralExpression)syntax);
                case SyntaxKind.AssignementExpression:
                    return BindAssignement((AssignementExpression) syntax);
                case SyntaxKind.VariableExpression:
                    return BindVariable((VariableExpression) syntax); 
                case SyntaxKind.IdentifierToken:
                    return BindIdentifier((IdentifierExpression) syntax);
                case SyntaxKind.EmptyExpression:
                    return new BoundEmptyExpression(-1);
                case SyntaxKind.EmptyListExpression:
                    return new BoundEmptyListExpression(-1);
                default:
                    Errs.ReportUnknown(syntax.Kind, 0);
                    return null;
            }
        }

        private BoundExpression BindIdentifier(IdentifierExpression syntax)
        {
            string name = syntax.Token.Text;
            if (!_scope.TryGetType(name, out Type t))
            {
                Errs.ReportUnknown(name, syntax.Token.Position);
                return null;
            }
            
            return new BoundVariable(name, t, syntax.Token.Position);
        }

        private BoundExpression BindVariable(VariableExpression syntax)
        {
            Type type = BoundVariableTypes.GetType(syntax.Token);
            
            if (type == null)
            {
                Errs.ReportUnknown(syntax.Token.Text, syntax.Token.Position);
                return new BoundVariable(syntax.Name.Token.Text, null, syntax.Token.Position);
            }
            
            if (!_scope.TryDeclare(syntax.Name.Token.Text, type))
            {
                Errs.ReportDeclared(syntax.Name.Token.Text, syntax.Token.Position);
                return null;
            }

            return new BoundVariable(syntax.Name.Token.Text, type, syntax.Token.Position);
        }

        private BoundExpression BindAssignement(AssignementExpression syntax)
        {
            BoundExpression assignement = BindExpression(syntax.Assignment);

            if (assignement == null)
                return null;

            BoundExpression assignee = BindExpression(syntax.Assignee);
            if (assignee is BoundVariable bv)
                return BindAssignmentVariable(assignement, bv, syntax);
            if (syntax.Token.Kind == SyntaxKind.AssignToken && IsUnpacking(assignee, assignement))
                return new BoundAssignmentUnpacking((BoundBinaryExpression)assignee, assignement, syntax.Token.Position);

            Errs.ReportUnexpected(typeof(BoundVariable), assignee, syntax.Token.Position);
            return null;
        }

        private bool IsUnpacking(BoundExpression assignee, BoundExpression assignment)
        {
            return (assignee is BoundBinaryExpression be && be.Operator.Kind == BoundBinaryKind.Comma && !(be.Left is BoundEmptyListExpression) &&
                    (assignment is BoundBinaryExpression abe && abe.Operator.Kind == BoundBinaryKind.Comma || 
                     assignment is BoundVariable bv && _scope.TryGetType(bv.Name, out Type t) && t == typeof(List)) 
                    && IsUnpackAssignee(be));
        }

        private bool IsUnpackAssignee(BoundBinaryExpression be)
        {
            return (be.Left is BoundVariable || (be.Left is BoundBinaryExpression be1 && IsUnpackAssignee(be1)) &&
                (be.Right is BoundEmptyExpression || be.Right is BoundVariable ||
                 be.Right is BoundBinaryExpression be2 && IsUnpackAssignee(be2)));
        }

        private BoundExpression BindAssignmentVariable(BoundExpression assignement, BoundVariable bv, AssignementExpression syntax)
        {
            if (bv.Type == null)
            {
                Errs.ReportUnknown(bv.Name, syntax.Token.Position);
                return null;
            }

            if (assignement.Type == typeof(object))
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
        

        private BoundExpression BindUnary(UnaryExpression syntax)
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

        private BoundExpression BindLiteral(LiteralExpression syntax)
        {
            return new BoundLiteral(syntax.Value ?? 0, syntax.Token.Position);
        }

        private BoundExpression BindBinary(BinaryExpression syntax)
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
            
            return new BoundBinaryExpression(left, bOperator, right, syntax.Token.Position);
        }
    }
}