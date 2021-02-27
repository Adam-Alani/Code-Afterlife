using System;
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
        private readonly BoundScope _scope;

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
        
        public BoundExpression BindExpression(ExpressionSyntax syntax)
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
                    return new BoundLiteral(0);
            }
        }

        private BoundExpression BindIdentifier(IdentifierExpression syntax)
        {
            string name = syntax.Token.Text;
            if (!_scope.TryGetType(name, out Type t))
            {
                Errs.ReportUnknown(name, syntax.Token.Position);
                return new BoundLiteral(0);
            }
            
            return new BoundVariable(name, t);
        }

        private BoundExpression BindVariable(VariableExpression syntax)
        {
            Type type = BoundVariableTypes.GetType(syntax.Token);
            
            if (type == null)
            {
                Errs.ReportUnknown(syntax.Token.Text, syntax.Token.Position);
                return new BoundLiteral(0);
            }
            
            if (!_scope.TryDeclare(syntax.Name.Token.Text, type))
            {
                Errs.ReportDeclared(syntax.Name.Token.Text, syntax.Token.Position);
                return new BoundLiteral(0);
            }

            return new BoundVariable(syntax.Name.Token.Text, type);
        }

        private BoundExpression BindAssignement(AssignementExpression syntax)
        {
            BoundExpression assignement = BindExpression(syntax.Assignment);
            
            BoundExpression assignee = BindExpression(syntax.Assignee);
            if (assignee is BoundVariable bv)
            {
                if (assignee.Type != assignement.Type && assignee.Type != typeof(object))
                {
                    Errs.ReportType(assignee.Type, assignement.Type, syntax.Token.Position);
                    return new BoundLiteral(0);
                }
                
                return new BoundAssignment(bv, assignement);
            }
            
            if (assignee != null)
                Errs.ReportUnknown(assignee, syntax.Token.Position);

            return new BoundLiteral(0);
        }
        

        private BoundExpression BindUnary(UnaryExpression syntax)
        {
            BoundExpression operandBound = BindExpression(syntax.Operand);
            BoundUnaryOperator uOperator = BoundUnaryOperator.Bind(syntax.Token.Kind, operandBound.Type);
            if (uOperator == null)
            {
                Errs.ReportUndefined(syntax.Token.Text, operandBound.Type, syntax.Token.Position);
                return operandBound;
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
            BoundBinaryOperator bOperator = BoundBinaryOperator.Bind(syntax.Token.Kind, left.Type, right.Type);

            if (bOperator == null)
            {
                Errs.ReportType(left.Type, right.Type, syntax.Token.Position);
                return left;
            }
            
            return new BoundBinary(left, bOperator, right);
        }
    }
}