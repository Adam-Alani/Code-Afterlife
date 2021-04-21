using System;
using System.Collections.Generic;
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
        private const int MAX_ITER = 250000;
        
        private const int MAX_DEPTH = 5000;
        
        private readonly BoundBlockStatement _root;
        private Scope _scope;

        public Errors Errs;
        public string StdOut;

        private int _depth;

        public Evaluator(BoundBlockStatement root, Scope scope)
        {
            _root = root;
            _scope = scope;
            Errs = new Errors();
            StdOut = "";
            _depth = 0;
        }
        
        public Evaluator(BoundBlockStatement root)
        {
            _root = root;
            _scope = new Scope();
            Errs = new Errors();
            StdOut = "";
            _depth = 0;
        }
        
        public Evaluator(BoundBlockStatement root, Scope scope, Errors errs)
        {
            _root = root;
            _scope = scope;
            Errs = errs;
            StdOut = "";
            _depth = 0;
        }
        
        public Evaluator(BoundBlockStatement root, Errors errs)
        {
            _root = root;
            _scope = new Scope();
            Errs = errs;
            StdOut = "";
            _depth = 0;
        }
        
        private object ReportError(string s, int p)
        {
            Errs.Report(s, p);
            return null;
        }

        public object Evaluate()
        {
            try
            {
                return EvaluateProgram(_root);
            }
            catch (Exception)
            {
                Errs.Report("Unexpected behaviour", -1);
                return null;
            }
        }

        private object EvaluateProgram(BoundBlockStatement program)
        {
            object lastValue = null;
            foreach (BoundStatement statement in program.Statements)
            {
                lastValue = EvaluateStatement(statement);
                if (_scope.Return) break;
            }
                
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
                BoundNodeKind.ReturnStatement => EvaluateReturn((BoundReturn) statement),
                _ => ReportError($"Unexpected statement {statement.Kind}", statement.Position)
            };
        }

        private object EvaluateReturn(BoundReturn statement)
        {
            _scope.Return = true;
            return EvaluateExpression(statement.Expression);
        }

        private object EvaluateFunctionDeclaration(BoundFunction statement)
        {
            Function f = new Function(statement.Args, statement.Body, statement.Type, _scope);
            EvaluateVariable(statement.Assignee);
            
            if (_scope.GetValue(statement.Assignee.Name) != null && _scope.GetValue(statement.Assignee.Name).GetType() != f.GetType())
            {
                Errs.ReportType(_scope?.GetValue(statement.Assignee.Name).GetType(), f.GetType(), statement.Position);
                return null;
            }
                
            _scope.SetValue(statement.Assignee.Name, f);
            return f;
        }

        private object EvaluateBlockStatement(BoundBlockStatement block)
        {
            _scope = new Scope(_scope);
            
            object lastValue = null;
            foreach (BoundStatement statement in block.Statements)
            {
                lastValue = EvaluateStatement(statement);
                if (_scope.Return) break;
            }
                

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
            uint calls = 0;
            for(; calls < MAX_ITER && (bool) EvaluateExpressionStatement(statement.Condition); calls++)
                lastValue = EvaluateStatement(statement.Then);
            
            if (calls == MAX_ITER)
                Errs.Report("Stack Overflow", statement.Position);
            return lastValue;
        }
        
        private object EvaluateForStatement(BoundFor statement)
        {
            _scope = new Scope(_scope);
            
            object lastValue = null;
            uint calls = 0;
            for(EvaluateExpressionStatement(statement.Initialisation); calls < MAX_ITER && (bool) EvaluateExpressionStatement(statement.Condition); EvaluateStatement(statement.Incrementation), calls++)
                lastValue = EvaluateStatement(statement.Then);
            
            _scope = _scope.Parent;
            
            if (calls == MAX_ITER)
                Errs.Report("Stack Overflow", statement.Position);
            
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
                BoundBinary b => EvaluateBinaryExpression(b),
                BoundEmptyExpression e => null,
                BoundEmptyListExpression el => new List(),
                BoundCallExpression ce => EvaluateCall(ce),
                BoundFunction bf => EvaluateFunctionDeclaration(bf),
                BoundIfExpression bif => EvaluateIfExpression(bif),
                _ => ReportError($"Unexpected node {expression.Kind}", expression.Position)
            };
        }

        private object EvaluateIfExpression(BoundIfExpression bif)
        {
            if ((bool)EvaluateExpression(bif.Condition))
                return EvaluateExpression(bif.Then);
            return EvaluateExpression(bif.Else);
        }

        private object EvaluateCall(BoundCallExpression ce)
        {
            if (_depth < MAX_DEPTH)
            {
                _depth += 1;
                Function f = (Function)EvaluateExpression(ce.Called);
                _scope = new Scope(_scope, f.Scope.Variables ?? new Dictionary<string, object>());


                if (ce.Args is BoundEmptyListExpression && !(f.Args is BoundEmptyListExpression))
                {
                    Errs.Report("Invalid call: not enough arguments", ce.Position);
                
                    _depth -= 1;
                    _scope = _scope.Parent;
                    return null;
                }

                if (f.Args is BoundEmptyListExpression && !(ce.Args is BoundEmptyListExpression))
                {
                    Errs.Report("Invalid call: too many arguments", ce.Position);
                
                    _depth -= 1;
                    _scope = _scope.Parent;
                    return null;
                }

                if (!(f.Args is BoundEmptyListExpression))
                {
                    BoundAssignmentUnpacking assignArgs =
                        new BoundAssignmentUnpacking((BoundBinary) f.Args, (BoundBinary) ce.Args, ce.Position);

                    EvaluateAssignmentUnpacking(assignArgs);
                }
            
                object val = EvaluateStatement(f.Body);
            
                _depth -= 1;
                _scope = _scope.Parent;
                return (f.Type != null) ? val : null;
            }

            Errs.Report("Stack overflow", ce.Position);
            return null;
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
            
            _scope.Undeclare("_");
            return assignments;
        }
        
        private List ConstructAssigneeList(BoundBinary assigneeList)
        {
            if (assigneeList.Left is BoundVariable bv)
            {
                return new List(bv,
                    (assigneeList.Right is BoundBinary bbe) ? ConstructAssigneeList(bbe) : 
                    (assigneeList.Right is BoundVariable bve) ? new List(bve) : null);
            }
            if (assigneeList.Left is BoundBinary bbv)
            {
                return new List(ConstructAssigneeList(bbv),
                    (assigneeList.Right is BoundBinary bbe) ? ConstructAssigneeList(bbe) : 
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
            BoundVariable bv = (BoundVariable) assignees.Head;
            object val = EvaluateVariable(bv);
            if (bv.Type == typeof(object) || val is List)
            {
                _scope.SetValue(bv.Name, assignments);
            }
            else
            {
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
                Errs.ReportType(_scope.GetValue(ba.Assignee.Name).GetType(), val?.GetType(), ba.Position);
                return null;
            }
                
            _scope.SetValue(ba.Assignee.Name, val);
            if (ba.Assignee.Name == "_") 
                _scope.Undeclare(ba.Assignee.Name);
            return val;
        }

        private object EvaluateUnaryExpression(BoundUnary u)
        {
            object operand = EvaluateExpression(u.Operand);
            
            if (BoundUnaryOperator.Bind(u.Operator.SyntaxKind, operand?.GetType() ?? typeof(object)) == null)
            {
                return ReportError($"Unexpected type {operand?.GetType()}", u.Position);
            }

            try
            {
                return u.Operator.Kind switch
                {
                    BoundUnaryKind.Neg => -(int) operand,
                    BoundUnaryKind.Id => operand,
                    BoundUnaryKind.Not => !((bool) operand),
                    BoundUnaryKind.Head => ((List) operand).Head,
                    BoundUnaryKind.Tail => ((List) operand).Tail,
                    BoundUnaryKind.Size => ((List) operand).Size,
                    BoundUnaryKind.Print => Print(operand),
                    _ => ReportError($"Unexpected unary operator {u.Operator.Kind}", u.Position)
                };
            }
            catch (Exception)
            {
                return ReportError("Invalid operation", u.Position);
            }
        }

        private object Print(object operand)
        {
            StdOut += operand.ToString();
            return operand.ToString();
        }

        private object EvaluateBinaryExpression(BoundBinary b)
        {
            object l = EvaluateExpression(b.Left);
            object r = EvaluateExpression(b.Right);

            if (BoundBinaryOperator.Bind(b.Operator.SyntaxKind, l?.GetType() ?? typeof(object), r?.GetType() ?? typeof(object)) == null)
            {
                Errs.ReportType("Incompatible types", l?.GetType(), r?.GetType(), b.Position);
                return null;
            }
            
            try
            {
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
                    BoundBinaryKind.Eq => CheckEquals(l, r),
                    BoundBinaryKind.Neq => !CheckEquals(l, r),
                    BoundBinaryKind.Gt => Convert.ToDouble(l) > Convert.ToDouble(r),
                    BoundBinaryKind.Lt => Convert.ToDouble(l) < Convert.ToDouble(r),
                    BoundBinaryKind.LtEq => Convert.ToDouble(l) <= Convert.ToDouble(r),
                    BoundBinaryKind.GtEq => Convert.ToDouble(l) >= Convert.ToDouble(r),
                    BoundBinaryKind.Comma => CreateList(l, r),
                    BoundBinaryKind.Dot => ListIndex((List)l, (int)r, b.Position),
                    _ => ReportError($"Unexpected binary operator {b.Operator.Kind}", b.Position)
                };
            }
            catch(DivideByZeroException)
            {
                return ReportError("Division by zero", b.Position);
            }
            catch (Exception)
            {
                return ReportError("Invalid operation", b.Position);
            }
        }

        private bool CheckEquals(object l, object r)
        {
            if (l?.GetType() == typeof(double) || r?.GetType() == typeof(double))
                return Math.Abs(Convert.ToDouble(l) - Convert.ToDouble(r)) < 0.000000001;
            return Equals(l, r);
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

        private object Add(object l, object r)
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
        
        private object Sub(object l, object r)
        {
            Type leftType = l.GetType();
            Type rightType = r.GetType();

            if (leftType == typeof(double) || rightType == typeof(double))
                return Convert.ToDouble(l) - Convert.ToDouble(r);

            return Convert.ToInt32(l) - Convert.ToInt32(r);
        }
        
        private object Mul(object l, object r)
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
        
        private object Div(object l, object r)
        {
            Type leftType = l.GetType();
            Type rightType = r.GetType();

            if (leftType == typeof(double) || rightType == typeof(double))
                return Convert.ToDouble(l) / Convert.ToDouble(r);

            return Convert.ToInt32(l) / Convert.ToInt32(r);
        }
        
        private object IntDiv(object l, object r)
        {
            return Convert.ToInt32(l) / Convert.ToInt32(r);
        }
    }
}