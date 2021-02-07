using System;

namespace AfterlifeInterpretor
{
    /// <summary>
    /// Evaluator class
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax root)
        {
            if (root is ParenthesisedExpression p)
                return EvaluateExpression(p.Expression);
            if (root is NumericExpression n)
                return (int)n.NumericToken.Value;
            if (root is BinaryExpression b)
            {
                int l = EvaluateExpression(b.Left);
                int r = EvaluateExpression(b.Right);

                return b.Token.Kind switch
                {
                    SyntaxKind.PlusToken   => l + r,
                    SyntaxKind.MinusToken  => l - r,
                    SyntaxKind.ModuloToken => l % r,
                    SyntaxKind.SlashToken  => l / r,
                    SyntaxKind.StarToken   => l * r,
                    _                      => throw new Exception($"Unexpected binary operator {b.Token.Kind}")
                };
            }

            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}