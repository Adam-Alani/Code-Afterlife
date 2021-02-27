using System;
using System.Collections.Generic;
using System.Linq;
using AfterlifeInterpretor.CodeAnalysis;
using AfterlifeInterpretor.CodeAnalysis.Syntax;
using AfterlifeInterpretor.CodeAnalysis.Binding;

namespace AfterlifeInterpretor
{
    public sealed class Interpretor
    {
        private readonly Scope _scope;
        public Dictionary<string, object> Variables => _scope.Variables;

        public Interpretor(Dictionary<string, object> variables)
        {
            _scope = new Scope(variables);
        }
        
        public Interpretor()
        {
            _scope = new Scope(new Dictionary<string, object>()); 
        }

        public EvaluationResults[] Interpret(string[] lines)
        {
            EvaluationResults[] res = new EvaluationResults[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                EvaluationResults eRes = Interpret(lines[i]);
                if (eRes.Errs.GetErrors().Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (Error error in eRes.Errs.GetErrors())
                    {
                        Console.WriteLine(error.ToString(i));
                    }
                    Console.ResetColor();
                }

                res[i] = eRes;
            }

            return res;
        }

        public EvaluationResults Interpret(string text)
        {
            SyntaxTree tree = new Parser(text).Parse();

            Binder binder = new Binder(_scope, tree.Errs);
            BoundExpression boundExpression = binder.BindExpression(tree.Root);
            
            Evaluator ev = new Evaluator(boundExpression, _scope);
            object res = (binder.Errs.GetErrors().Any()) ? null : ev.Evaluate();

            return new EvaluationResults(binder.Errs, res);
        }
    }
}