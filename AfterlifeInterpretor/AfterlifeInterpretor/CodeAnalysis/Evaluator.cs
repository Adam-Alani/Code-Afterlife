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

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression root)
        {
            if (root is BoundLiteral n)
                return (int)n.Value;
            if (root is BoundUnary u)
            {
                return u.OperatorKind switch
                {
                    BoundUnaryKind.Neg => -EvaluateExpression(u.Operand),
                    BoundUnaryKind.Id => EvaluateExpression(u.Operand),
                    _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}")
                };
            }
            if (root is BoundBinary b)
            {
                int l = EvaluateExpression(b.Left);
                int r = EvaluateExpression(b.Right);

                return b.OperatorKind switch
                {
                    BoundBinaryKind.Add   => l + r,
                    BoundBinaryKind.Sub  => l - r,
                    BoundBinaryKind.Mod => l % r,
                    BoundBinaryKind.Div  => l / r,
                    BoundBinaryKind.Mul   => l * r,
                    _                      => throw new Exception($"Unexpected binary operator {b.OperatorKind}")
                };
            }

            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}