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
        public Dictionary<string, object> Variables { get; }

        public Interpretor(Dictionary<string, object> variables)
        {
            Variables = variables;
        }
        
        public Interpretor()
        {
            Variables = new Dictionary<string, object>();
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

            Binder binder = new Binder(Variables, tree.Errs);
            BoundExpression boundExpression = binder.BindExpression(tree.Root);
            
            Evaluator ev = new Evaluator(boundExpression, Variables);
            object res = (binder.Errs.GetErrors().Any()) ? null : ev.Evaluate();

            return new EvaluationResults(binder.Errs, res);
        }
    }
}