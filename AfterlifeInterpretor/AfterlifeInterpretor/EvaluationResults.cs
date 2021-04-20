using AfterlifeInterpretor.CodeAnalysis;

namespace AfterlifeInterpretor
{
    public sealed class EvaluationResults
    {
        public Errors Errs { get; }
        public string StdOut { get; }
        public object Value { get; }

        public EvaluationResults(Errors errs, string stdout, object value)
        {
            Errs = errs;
            StdOut = stdout;
            Value = value;
        }
    }
}