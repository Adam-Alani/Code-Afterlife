using System;
using AfterlifeInterpretor.CodeAnalysis.Binding;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

namespace AfterlifeInterpretor.CodeAnalysis
{
    /// <summary>
    /// Evaluator class
    /// Evaluates a given code
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression root)
        {
            if (root is BoundLiteral n)
                return n.Value;
            if (root is BoundUnary u)
            {
                object operand = EvaluateExpression(u.Operand);
                return u.Operator.Kind switch
                {
                    BoundUnaryKind.Neg => -(int)operand,
                    BoundUnaryKind.Id => (int)operand,
                    BoundUnaryKind.Not => !((bool)operand),
                    _ => throw new Exception($"Unexpected unary operator {u.Operator.Kind}")
                };
            }
            if (root is BoundBinary b)
            {
                object l = EvaluateExpression(b.Left);
                object r = EvaluateExpression(b.Right);

                return b.Operator.Kind switch
                {
                    BoundBinaryKind.Add   => (int)l + (int)r,
                    BoundBinaryKind.Sub  => (int)l - (int)r,
                    BoundBinaryKind.Mod => (int)l % (int)r,
                    BoundBinaryKind.Div  => (int)l / (int)r,
                    BoundBinaryKind.Mul   => (int)l * (int)r,
                    BoundBinaryKind.And => (bool)l && (bool)r,
                    BoundBinaryKind.Or => (bool)l || (bool)r,
                    BoundBinaryKind.Eq => Equals(l, r),
                    BoundBinaryKind.Neq => !Equals(l, r),
                    BoundBinaryKind.Gt => (int)l > (int)r,
                    BoundBinaryKind.Lt => (int)l < (int)r,
                    BoundBinaryKind.LtEq => (int)l <= (int)r,
                    BoundBinaryKind.GtEq => (int)l >= (int)r,
                    _                      => throw new Exception($"Unexpected binary operator {b.Operator.Kind}")
                };
            }

            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}