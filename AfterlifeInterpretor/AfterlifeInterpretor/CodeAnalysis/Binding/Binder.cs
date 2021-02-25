using System;
using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly Dictionary<string, object> _variables;
        
        public List<string> Errors = new List<string>();
        
        public Binder(Dictionary<string, object> variables)
        {
            _variables = variables;
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
                    Errors.Add($"Unexpected syntax: {syntax.Kind}");
                    return null;
                    // throw new Exception($"Unexpected syntax: {syntax.Kind}");
            }
        }

        private BoundExpression BindIdentifier(IdentifierExpression syntax)
        {
            string name = syntax.Token.Text;
            if (!_variables.TryGetValue(name, out object value))
            {
                Errors.Add($"Unknown variable: {name} at {syntax.Token.Position}");
                return null;
                // throw new Exception($"Unknown variable: {name} at {syntax.Token.Position}");
            }
            
            return new BoundVariable(name, value.GetType());
        }

        private BoundExpression BindVariable(VariableExpression syntax)
        {
            if (_variables.ContainsKey(syntax.Name.Token.Text))
            {
                Errors.Add($"Error: {syntax.Name.Token.Text} has already been declared at {syntax.Token.Position}");
                return null;
            }
            
            Type type = BoundVariableTypes.GetType(syntax.Token);

            if (type == null)
            {
                Errors.Add($"$Unknown type: {syntax.Token.Text} at {syntax.Token.Position}");
                return null;
                // throw new Exception($"$Unknown type: {syntax.Token.Text} at {syntax.Token.Position}");
            }
            
            return new BoundVariable(syntax.Name.Token.Text, type);
        }

        private BoundExpression BindAssignement(AssignementExpression syntax)
        {
            BoundExpression assignee = BindExpression(syntax.Assignee);

            if (assignee is BoundVariable bv)
            {
                BoundExpression assignement = BindExpression(syntax.Assignment);
                if (assignee.Type != assignement.Type && assignee.Type != typeof(object))
                {
                    Errors.Add($"Error: Cannot assign {assignee.Type} with {assignement.Type} at {syntax.Token.Position}");
                    return null;
                    // throw new Exception($"Error: Invalid variable at {syntax.Token.Position}");
                }
                
                return new BoundAssignment(bv, assignement);
            }
            
            if (assignee != null)
                Errors.Add($"Error: Invalid variable at {syntax.Token.Position}");
            return null;
            //throw new Exception($"Error: Invalid variable at {syntax.Token.Position}");
        }
        

        private BoundExpression BindUnary(UnaryExpression syntax)
        {
            BoundExpression operandBound = BindExpression(syntax.Operand);
            BoundUnaryOperator uOperator = BoundUnaryOperator.Bind(syntax.Token.Kind, operandBound.Type);
            if (uOperator == null)
            {
                Errors.Add($"Error: Unary operator {syntax.Token.Text} is not defined for {operandBound.Type} at {syntax.Token.Position}");
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
                Errors.Add($"Error: Binary operator {syntax.Token.Text} is not defined for {left.Type} and {right.Type} at {syntax.Token.Position}");
                return left;
            }
            
            return new BoundBinary(left, bOperator, right);
        }
    }
}