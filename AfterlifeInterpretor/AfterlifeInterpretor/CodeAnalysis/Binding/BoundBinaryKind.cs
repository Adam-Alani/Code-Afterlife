namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal enum BoundBinaryKind
    {
        // Int binary
        Add,
        Sub,
        Mul,
        Div,
        IntDiv,
        Mod,
        
        // Bool binary
        And,
        Or,
        Eq,
        Neq,
        Gt,
        Lt,
        GtEq,
        LtEq,
        
        // List binary
        Comma,
        Dot
    }
}