namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal enum BoundUnaryKind
    {
        // Int unary
        Id,
        Neg,
        
        // Bool unary
        Not,
        
        // List unary
        Head,
        Tail,
        Size,
        
        // Other
        Print
    }
}