using System;
using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

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
            
            return new BoundBlockStatement(statements.ToArray());
        }

        private BoundStatement BindStatement(StatementSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement((BlockStatement) syntax);
                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement((ExpressionStatement) syntax);
                default:
                    Errs.ReportUnknown(syntax.Kind, 0);
                    return null;
            }
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
            
            return new BoundBlockStatement(statements.ToArray());
        }
        
        private BoundStatement BindExpressionStatement(ExpressionStatement syntax)
        {
            return new BoundExpressionStatement(BindExpression(syntax.Expression));
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
            
            return new BoundVariable(name, t);
        }

        private BoundExpression BindVariable(VariableExpression syntax)
        {
            Type type = BoundVariableTypes.GetType(syntax.Token);
            
            if (type == null)
            {
                Errs.ReportUnknown(syntax.Token.Text, syntax.Token.Position);
                return new BoundVariable(syntax.Name.Token.Text, null);
            }
            
            if (!_scope.TryDeclare(syntax.Name.Token.Text, type))
            {
                Errs.ReportDeclared(syntax.Name.Token.Text, syntax.Token.Position);
                return null;
            }

            return new BoundVariable(syntax.Name.Token.Text, type);
        }

        private BoundExpression BindAssignement(AssignementExpression syntax)
        {
            BoundExpression assignement = BindExpression(syntax.Assignment);

            if (assignement == null)
                return null;
            
            BoundExpression assignee = BindExpression(syntax.Assignee);
            if (assignee is BoundVariable bv)
            {
                if (bv.Type == null)
                {
                    Errs.ReportUnknown(bv.Name, syntax.Token.Position);
                    return null;
                }
                
                if (assignee.Type != assignement.Type && assignee.Type != typeof(object))
                {
                    Errs.ReportType(syntax.Token.Text, assignee.Type, assignement.Type, syntax.Token.Position);
                    return null;
                }
                
                if (assignee.Type == typeof(object))
                    _scope.ChangeType(bv.Name, assignement.Type);
                return new BoundAssignment(bv, assignement);
            }

            return null;
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
            return new BoundUnary(uOperator, operandBound);
        }

        private BoundExpression BindLiteral(LiteralExpression syntax)
        {
            return new BoundLiteral(syntax.Value ?? 0);
        }

        private BoundExpression BindBinary(BinaryExpression syntax)
        {
            BoundExpression left = BindExpression(syntax.Left);
            BoundExpression right = BindExpression(syntax.Right);

            if (left == null || right == null)
                return null;
            
            BoundBinaryOperator bOperator = BoundBinaryOperator.Bind(syntax.Token.Kind, left.Type, right.Type);

            if (bOperator == null)
            {
                Errs.ReportType(syntax.Token.Text, left.Type, right.Type, syntax.Token.Position);
                return null;
            }
            
            return new BoundBinary(left, bOperator, right);
        }
    }
}