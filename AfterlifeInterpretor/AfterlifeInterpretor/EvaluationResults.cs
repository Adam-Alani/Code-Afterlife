using System.Collections.Generic;

namespace AfterlifeInterpretor
{
    public sealed class EvaluationResults
    {
        public string[] Errors { get; }
        public object Value { get; }

        public EvaluationResults(string[] errors, object value)
        {
            Errors = errors;
            Value = value;
        }
    }
}