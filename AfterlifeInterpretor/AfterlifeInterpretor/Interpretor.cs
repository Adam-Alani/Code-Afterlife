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

        public EvaluationResults Evaluate(string text)
        {
            SyntaxTree tree = new Parser(text).Parse();
            List<string> errors = tree.Errors;
            if (errors.Any())
                return new EvaluationResults(errors.ToArray(), null);
            Binder binder = new Binder(Variables);
            BoundExpression boundExpression = binder.BindExpression(tree.Root);
            if (errors.Concat(binder.Errors).Any())
                return new EvaluationResults(errors.Concat(binder.Errors).ToArray(), null);

            return new EvaluationResults(errors.Concat(binder.Errors).ToArray(), new Evaluator(boundExpression, Variables).Evaluate());
        }
    }
}