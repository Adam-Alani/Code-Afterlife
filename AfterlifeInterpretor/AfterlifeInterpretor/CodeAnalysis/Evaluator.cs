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
                return u.OperatorKind switch
                {
                    BoundUnaryKind.Neg => -(int)operand,
                    BoundUnaryKind.Id => (int)operand,
                    BoundUnaryKind.Not => !((bool)operand),
                    _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}")
                };
            }
            if (root is BoundBinary b)
            {
                object l = EvaluateExpression(b.Left);
                object r = EvaluateExpression(b.Right);

                return b.OperatorKind switch
                {
                    BoundBinaryKind.Add   => (int)l + (int)r,
                    BoundBinaryKind.Sub  => (int)l - (int)r,
                    BoundBinaryKind.Mod => (int)l % (int)r,
                    BoundBinaryKind.Div  => (int)l / (int)r,
                    BoundBinaryKind.Mul   => (int)l * (int)r,
                    BoundBinaryKind.And => (bool)l && (bool)r,
                    BoundBinaryKind.Or => (bool)l || (bool)r,
                    _                      => throw new Exception($"Unexpected binary operator {b.OperatorKind}")
                };
            }

            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}