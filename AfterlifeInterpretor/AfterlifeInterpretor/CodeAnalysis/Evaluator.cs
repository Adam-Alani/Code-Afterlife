using System;
using AfterlifeInterpretor.CodeAnalysis.Binding;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Parser;

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

        public Errors Errs;
        public string StdOut;

        public Evaluator(BoundBlockStatement root, Scope scope)
        {
            _root = root;
            _scope = scope;
            Errs = new Errors();
            StdOut = "";
        }
        
        public Evaluator(BoundBlockStatement root)
        {
            _root = root;
            _scope = new Scope();
            Errs = new Errors();
            StdOut = "";
        }
        
        public Evaluator(BoundBlockStatement root, Scope scope, Errors errs)
        {
            _root = root;
            _scope = scope;
            Errs = errs;
            StdOut = "";
        }
        
        public Evaluator(BoundBlockStatement root, Errors errs)
        {
            _root = root;
            _scope = new Scope();
            Errs = errs;
            StdOut = "";
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
                BoundNodeKind.IfStatement => EvaluateIfStatement((BoundIf) statement),
                BoundNodeKind.WhileStatement => EvaluateWhileStatement((BoundWhile) statement),
                BoundNodeKind.ForStatement => EvaluateForStatement((BoundFor) statement),
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

        private object EvaluateIfStatement(BoundIf statement)
        {
            if ((bool) EvaluateExpressionStatement(statement.Condition))
                return EvaluateStatement(statement.Then);
            return statement.Else != null ? EvaluateStatement(statement.Else) : null;
        }
        
        private object EvaluateWhileStatement(BoundWhile statement)
        {
            object lastValue = null;
            for(uint calls = 0; calls < 250000 && (bool) EvaluateExpressionStatement(statement.Condition); calls++)
                lastValue = EvaluateStatement(statement.Then);
            return lastValue;
        }
        
        private object EvaluateForStatement(BoundFor statement)
        {
            _scope = new Scope(_scope);
            
            object lastValue = null;
            uint calls = 0;
            for(EvaluateExpressionStatement(statement.Initialisation); calls < 250000 && (bool) EvaluateExpressionStatement(statement.Condition); EvaluateStatement(statement.Incrementation), calls++)
                lastValue = EvaluateStatement(statement.Then);
            
            _scope = _scope.Parent;
            return lastValue;
        }


        private object EvaluateExpression(BoundExpression expression)
        {
            return expression switch
            {
                BoundLiteral n => n.Value,
                BoundVariable bv => EvaluateVariable(bv),
                BoundAssignment ba => EvaluateAssignment(ba),
                BoundAssignmentUnpacking bau => EvaluateAssignmentUnpacking(bau),
                BoundUnary u => EvaluateUnaryExpression(u),
                BoundBinaryExpression b => EvaluateBinaryExpression(b),
                BoundEmptyExpression e => null,
                BoundEmptyListExpression el => new List(),
                _ => throw new Exception($"Unexpected node {expression.Kind}")
            };
        }

        private object EvaluateAssignmentUnpacking(BoundAssignmentUnpacking bau)
        {
            List assignments = (List) EvaluateExpression(bau.Assignment);
            List assignees = ConstructAssigneeList(bau.Assignee);

            if (assignees.Size > assignments.Size)
            {
                Errs.Report("Cannot unpack: Not enough elements", bau.Position);
                return null;
            }
            
            UnpackAssignments(assignees, assignments, bau);
            
            return assignments;
        }
        
        private List ConstructAssigneeList(BoundBinaryExpression assigneeList)
        {
            if (assigneeList.Left is BoundVariable bv)
            {
                return new List(bv,
                    (assigneeList.Right is BoundBinaryExpression bbe) ? ConstructAssigneeList(bbe) : 
                    (assigneeList.Right is BoundVariable bve) ? new List(bve) : null);
            }
            if (assigneeList.Left is BoundBinaryExpression bbv)
            {
                return new List(ConstructAssigneeList(bbv),
                    (assigneeList.Right is BoundBinaryExpression bbe) ? ConstructAssigneeList(bbe) : 
                    (assigneeList.Right is BoundVariable bve) ? new List(bve) : null);
            }
            
            Errs.Report("Invalid unpacking", assigneeList.Position);
            return null;
        }

        private void UnpackAssignments(List assignees, List assignments, BoundAssignmentUnpacking bau)
        {
            if (assignees.Size > assignments.Size)
            {
                Errs.Report("Cannot unpack: Not enough elements", bau.Position);
                return;
            }
            
            if (assignees.Tail.IsEmpty)
            {
                AssignTail(assignees, assignments, bau);
            }
            else
            {
                AssignHead(assignees, assignments, bau);
            }
        }

        private void AssignTail(List assignees, List assignments, BoundAssignmentUnpacking bau)
        {
            if (assignees.Head is List al)
            {
                UnpackAssignments(al, assignments, bau);
            }
            else if (assignees.Head is BoundVariable bv)
            {
                EvaluateVariable(bv);
                object isAssigned = (assignments.Tail.IsEmpty) ? assignments.Head : assignments;
                if (_scope.GetValue(bv.Name) != null && _scope.GetValue(bv.Name).GetType() != isAssigned?.GetType())
                {
                    Errs.ReportType(_scope?.GetValue(bv.Name).GetType(), isAssigned?.GetType(), bau.Position);
                    return;
                }
                
                _scope.SetValue(bv.Name, isAssigned);
            }
        }

        private void AssignHead(List assignees, List assignments, BoundAssignmentUnpacking bau)
        {
            if (assignees.Head is List al)
            {
                if (assignments.Head is List ah)
                    UnpackAssignments(al, ah, bau);
                else
                {
                    Errs.ReportType(typeof(List), assignments.Head?.GetType(), bau.Position);
                    return;
                }
            }
            else if (assignees.Head is BoundVariable bv)
            {
                EvaluateVariable(bv);
                if (_scope.GetValue(bv.Name) != null && _scope.GetValue(bv.Name).GetType() != assignments.Head?.GetType())
                {
                    Errs.ReportType(_scope?.GetValue(bv.Name).GetType(), assignments.Head?.GetType(), bau.Position);
                    return;
                }
                
                _scope.SetValue(bv.Name, assignments.Head);
            }
            UnpackAssignments(assignees.Tail, assignments.Tail, bau);
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
            if (_scope.GetValue(ba.Assignee.Name) != null && _scope.GetValue(ba.Assignee.Name).GetType() != val?.GetType())
            {
                Errs.ReportType(_scope?.GetValue(ba.Assignee.Name).GetType(), val?.GetType(), ba.Position);
                return null;
            }
                
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
                BoundUnaryKind.Head => ((List) operand).Head,
                BoundUnaryKind.Tail => ((List) operand).Tail,
                BoundUnaryKind.Size => ((List) operand).Size,
                BoundUnaryKind.Print => Print(operand),
                _ => throw new Exception($"Unexpected unary operator {u.Operator.Kind}")
            };
        }

        private object Print(object operand)
        {
            StdOut += operand.ToString();
            return operand.ToString();
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            object l = EvaluateExpression(b.Left);
            object r = EvaluateExpression(b.Right);

            return b.Operator.Kind switch
            {
                BoundBinaryKind.Add => Add(l, r),
                BoundBinaryKind.Sub => Sub(l, r),
                BoundBinaryKind.Mod => Convert.ToInt32(l) % Convert.ToInt32(r),
                BoundBinaryKind.Div => Div(l, r),
                BoundBinaryKind.IntDiv => IntDiv(l, r),
                BoundBinaryKind.Mul => Mul(l, r),
                BoundBinaryKind.And => (bool) l && (bool) r,
                BoundBinaryKind.Or => (bool) l || (bool) r,
                BoundBinaryKind.Eq => Equals(l, r),
                BoundBinaryKind.Neq => !Equals(l, r),
                BoundBinaryKind.Gt => (int) l > (int) r,
                BoundBinaryKind.Lt => (int) l < (int) r,
                BoundBinaryKind.LtEq => (int) l <= (int) r,
                BoundBinaryKind.GtEq => (int) l >= (int) r,
                BoundBinaryKind.Comma => CreateList(l, r),
                BoundBinaryKind.Dot => ListIndex((List)l, (int)r, b.Position),
                _ => throw new Exception($"Unexpected binary operator {b.Operator.Kind}")
            };
        }

        private object ListIndex(List list, int i, int pos)
        {
            if (i >= list.Size)
            {
                Errs.Report("Index Out Of Bounds", pos);
                return null;
            }

            return list[i];
        }

        private List CreateList(object l, object r)
        {
            List child = null;

            if (r is List list)
                child = list;
            else if (r != null)
                child = new List(r);

            return new List(l, child);
        }

        private static object GetDefault(Type bvType)
        {
            if (bvType == typeof(bool))
                return false;
            if (bvType == typeof(int))
                return 0;
            if (bvType == typeof(double))
                return 0.0;
            if (bvType == typeof(string))
                return "";
            if (bvType == typeof(List))
                return new List();
            return null;
        }

        private static object Add(object l, object r)
        {
            Type leftType = l.GetType();
            Type rightType = r.GetType();

            if (leftType == typeof(List))
                return (List) l + r;
            if (rightType == typeof(List))
                return l + (List) r;

            if (leftType == typeof(string) || rightType == typeof(string))
                return Convert.ToString(l) + Convert.ToString(r);
            
            if (leftType == typeof(double) || rightType == typeof(double))
                return Convert.ToDouble(l) + Convert.ToDouble(r);
            
            
            return Convert.ToInt32(l) + Convert.ToInt32(r);
        }
        
        private static object Sub(object l, object r)
        {
            Type leftType = l.GetType();
            Type rightType = r.GetType();

            if (leftType == typeof(double) || rightType == typeof(double))
                return Convert.ToDouble(l) - Convert.ToDouble(r);

            return Convert.ToInt32(l) - Convert.ToInt32(r);
        }
        
        private static object Mul(object l, object r)
        {
            Type leftType = l.GetType();
            Type rightType = r.GetType();

            if (leftType == typeof(string))
            {
                int n = Convert.ToInt32(r);
                string a = "";
                while (n-- > 0)
                {
                    a += Convert.ToString(l);
                }

                return a;
            }
            
            if (rightType == typeof(string))
            {
                int n = Convert.ToInt32(l);
                string a = "";
                while (n-- > 0)
                {
                    a += Convert.ToString(r);
                }

                return a;
            }

            if (leftType == typeof(double) || rightType == typeof(double))
                return Convert.ToDouble(l) * Convert.ToDouble(r);

            return Convert.ToInt32(l) * Convert.ToInt32(r);
        }
        
        private static object Div(object l, object r)
        {
            Type leftType = l.GetType();
            Type rightType = r.GetType();

            if (leftType == typeof(double) || rightType == typeof(double))
                return Convert.ToDouble(l) / Convert.ToDouble(r);

            return Convert.ToInt32(l) / Convert.ToInt32(r);
        }
        
        private static object IntDiv(object l, object r)
        {
            return Convert.ToInt32(l) / Convert.ToInt32(r);
        }
    }
}