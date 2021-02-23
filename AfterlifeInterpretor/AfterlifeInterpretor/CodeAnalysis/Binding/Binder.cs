using System;
using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        public List<string> Errors = new List<string>();
        
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
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindUnary(UnaryExpression syntax)
        {
            BoundExpression operandBound = BindExpression(syntax.Operand);
            BoundUnaryOperator uOperator = BoundUnaryOperator.Bind(syntax.Token.Kind, operandBound.Type);
            if (uOperator == null)
            {
                Errors.Add($"Unary operator {syntax.Token.Text} is not defined for {operandBound.Type}");
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
                Errors.Add($"Binary operator {syntax.Token.Text} is not defined for {left.Type} and {right.Type}");
                return left;
            }
            
            return new BoundBinary(left, bOperator, right);
        }
    }
}