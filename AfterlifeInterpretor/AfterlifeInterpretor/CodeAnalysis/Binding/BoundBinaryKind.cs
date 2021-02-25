namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal enum BoundBinaryKind
    {
        // Int binary
        Add,
        Sub,
        Mul,
        Div,
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
        
        // Assign 
        Assign
    }
}