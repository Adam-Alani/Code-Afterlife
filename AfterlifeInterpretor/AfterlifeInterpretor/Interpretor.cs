using System;
using System.Collections.Generic;
using System.Linq;
using AfterlifeInterpretor.CodeAnalysis;
using AfterlifeInterpretor.CodeAnalysis.Binding;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Parser;

namespace AfterlifeInterpretor
{
    public sealed class Interpretor
    {
        private Scope _scope;
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
            Scope newScope = new Scope(_scope.Parent, new Dictionary<string, object>(_scope.Variables));
            SyntaxTree tree = new Parser(text).Parse();

            Binder binder = new Binder(newScope, tree.Errs);
            BoundBlockStatement bound = binder.BindProgram(tree.Root);
            
            Evaluator ev = new Evaluator(bound, newScope, binder.Errs);
            object res = (binder.Errs.GetErrors().Any()) ? null : ev.Evaluate();

            if (!ev.Errs.GetErrors().Any())
                _scope = newScope;
            return new EvaluationResults(ev.Errs, ev.StdOut, res);
        }
    }
}