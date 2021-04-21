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

        public override string ToString()
        {
            if (Value is null)
                return "()";
            if (Value is string)
                return '"' + Value.ToString() + '"';
            return Value.ToString();
        }
    }
}