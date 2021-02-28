using System;
using AfterlifeInterpretor.CodeAnalysis.Binding;

namespace AfterlifeInterpretor.CodeAnalysis
{
    /// <summary>
    /// Evaluator class
    /// Evaluates a given code
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal sealed class Evaluator
    {
        private readonly BoundBlockStatement _root;
        private Scope _scope;

        public Evaluator(BoundBlockStatement root, Scope scope)
        {
            _root = root;
            _scope = scope;
        }
        
        public Evaluator(BoundBlockStatement root)
        {
            _root = root;
            _scope = new Scope();
        }

        public object Evaluate()
        {
            return EvaluateProgram(_root);
        }

        private object EvaluateProgram(BoundBlockStatement program)
        {
            object lastValue = null;
            foreach (BoundStatement statement in program.Statements)
                lastValue = EvaluateStatement(statement);
            return lastValue;
        }

        private object EvaluateStatement(BoundStatement statement)
        {
            return statement.Kind switch
            {
                BoundNodeKind.BlockStatement => EvaluateBlockStatement((BoundBlockStatement) statement),
                BoundNodeKind.ExpressionStatement => EvaluateExpressionStatement((BoundExpressionStatement) statement),
                _ => throw new Exception($"Unexpected statement {statement.Kind}")
            };
        }

        private object EvaluateBlockStatement(BoundBlockStatement block)
        {
            _scope = new Scope(_scope);
            
            object lastValue = null;
            foreach (BoundStatement statement in block.Statements)
                lastValue = EvaluateStatement(statement);

            _scope = _scope.Parent;
            return lastValue;
        }
        
        private object EvaluateExpressionStatement(BoundExpressionStatement statement)
        {
            return EvaluateExpression(statement.Expression); 
        }


        private object EvaluateExpression(BoundExpression expression)
        {
            return expression switch
            {
                BoundLiteral n => n.Value,
                BoundVariable bv => EvaluateVariable(bv),
                BoundAssignment ba => EvaluateAssignment(ba),
                BoundUnary u => EvaluateUnaryExpression(u),
                BoundBinary b => EvaluateBinaryExpression(b),
                _ => throw new Exception($"Unexpected node {expression.Kind}")
            };
        }

        private object EvaluateVariable(BoundVariable bv)
        {
            _scope.Declare(bv.Name, GetDefault(bv.Type));
            return _scope.GetValue(bv.Name);
        }

        private object EvaluateAssignment(BoundAssignment ba)
        {
            EvaluateExpression(ba.Assignee); // Declaring things on the left side when needed
            object val = EvaluateExpression(ba.Assignment);
            _scope.SetValue(ba.Assignee.Name, val);
            return val;
        }

        private object EvaluateUnaryExpression(BoundUnary u)
        {
            object operand = EvaluateExpression(u.Operand);
            return u.Operator.Kind switch
            {
                BoundUnaryKind.Neg => -(int) operand,
                BoundUnaryKind.Id => operand,
                BoundUnaryKind.Not => !((bool) operand),
                _ => throw new Exception($"Unexpected unary operator {u.Operator.Kind}")
            };
        }

        private object EvaluateBinaryExpression(BoundBinary b)
        {
            object l = EvaluateExpression(b.Left);
            object r = EvaluateExpression(b.Right);

            return b.Operator.Kind switch
            {
                BoundBinaryKind.Add => (int) l + (int) r,
                BoundBinaryKind.Sub => (int) l - (int) r,
                BoundBinaryKind.Mod => (int) l % (int) r,
                BoundBinaryKind.Div => (int) l / (int) r,
                BoundBinaryKind.Mul => (int) l * (int) r,
                BoundBinaryKind.And => (bool) l && (bool) r,
                BoundBinaryKind.Or => (bool) l || (bool) r,
                BoundBinaryKind.Eq => Equals(l, r),
                BoundBinaryKind.Neq => !Equals(l, r),
                BoundBinaryKind.Gt => (int) l > (int) r,
                BoundBinaryKind.Lt => (int) l < (int) r,
                BoundBinaryKind.LtEq => (int) l <= (int) r,
                BoundBinaryKind.GtEq => (int) l >= (int) r,
                _ => throw new Exception($"Unexpected binary operator {b.Operator.Kind}")
            };
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