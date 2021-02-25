using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Dictionary<string, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<string, object> variables)
        {
            _root = root;
            _variables = variables;
        }
        
        public Evaluator(BoundExpression root)
        {
            _root = root;
            _variables = new Dictionary<string, object>();
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression root)
        {
            if (root is BoundLiteral n)
                return n.Value;
            if (root is BoundVariable bv)
            {
                if (!_variables.ContainsKey(bv.Name))
                {
                    _variables.Add(bv.Name, GetDefault(bv.Type));
                }
                
                return _variables[bv.Name];
            }

            if (root is BoundAssignment ba)
            {
                EvaluateExpression(ba.Assignee); // Declaring things on the left side when needed
                _variables[ba.Assignee.Name] = EvaluateExpression(ba.Assignment);
                return _variables[ba.Assignee.Name];
            }
            if (root is BoundUnary u)
            {
                object operand = EvaluateExpression(u.Operand);
                switch (u.Operator.Kind)
                {
                    case BoundUnaryKind.Neg:
                        return -(int) operand;
                    case BoundUnaryKind.Id:
                        return (int) operand;
                    case BoundUnaryKind.Not:
                        return !((bool) operand);
                    case BoundUnaryKind.Bool:
                        
                    default:
                        throw new Exception($"Unexpected unary operator {u.Operator.Kind}");
                }
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

        private static object GetDefault(Type bvType)
        {
            if (bvType == typeof(bool))
                return false;
            if (bvType == typeof(int))
                return 0;
            return null;
        }
    }
}