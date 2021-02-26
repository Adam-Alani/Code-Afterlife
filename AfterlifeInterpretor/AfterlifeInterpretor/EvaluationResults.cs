using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis;

namespace AfterlifeInterpretor
{
    public sealed class EvaluationResults
    {
        public Errors Errs { get; }
        public object Value { get; }

        public EvaluationResults(Errors errs, object value)
        {
            Errs = errs;
            Value = value;
        }
    }
}