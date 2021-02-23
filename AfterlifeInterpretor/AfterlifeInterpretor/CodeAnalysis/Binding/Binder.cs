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
        
        private BoundUnaryKind? BindUnaryKind(SyntaxKind kind, Type type)
        {
            if (type != typeof(int))
                return null;
            
            return kind switch
            {
                SyntaxKind.MinusToken => BoundUnaryKind.Neg,
                SyntaxKind.PlusToken => BoundUnaryKind.Id,
                _ => throw new Exception($"Unexpected unary operator {kind}")
            };
        }
        
        private BoundBinaryKind? BindBinaryKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType != typeof(int) || rightType != typeof(int))
                return null;
            
            return kind switch
            {
                SyntaxKind.MinusToken => BoundBinaryKind.Sub,
                SyntaxKind.PlusToken => BoundBinaryKind.Add,
                SyntaxKind.SlashToken => BoundBinaryKind.Div,
                SyntaxKind.StarToken => BoundBinaryKind.Mul,
                SyntaxKind.ModuloToken => BoundBinaryKind.Mod,
                _ => throw new Exception($"Unexpected binary operator {kind}")
            };
        }

        private BoundExpression BindUnary(UnaryExpression syntax)
        {
            BoundExpression operandBound = BindExpression(syntax.Operand);
            BoundUnaryKind? kind = BindUnaryKind(syntax.Operand.Kind, operandBound.Type);
            if (kind == null)
            {
                Errors.Add($"Unary operator {syntax.Token.Text} is not defined for {operandBound.Type}");
                return operandBound;
            }
            return new BoundUnary((BoundUnaryKind)kind, operandBound);
        }

        private BoundExpression BindLiteral(LiteralExpression syntax)
        {
            return new BoundLiteral(syntax.Token.Value == null ? 0 : syntax.Token.Value);
        }

        private BoundExpression BindBinary(BinaryExpression syntax)
        {
            BoundExpression left = BindExpression(syntax.Left);
            BoundExpression right = BindExpression(syntax.Right);
            BoundBinaryKind? kind = BindBinaryKind(syntax.Token.Kind, left.Type, right.Type);

            if (kind == null)
            {
                Errors.Add($"Unary operator {syntax.Token.Text} is not defined for {left.Type} and {right.Type}");
                return left;
            }
            
            return new BoundBinary(left, (BoundBinaryKind)kind, right);
        }
    }
}