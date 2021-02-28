
namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    /// <summary>
    /// SyntaxTree class
    /// A tree parsed by a parser
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public sealed class SyntaxTree
    {
        public Errors Errs { get; }
        public BlockStatement Root { get; }
        public SyntaxToken End { get;  }
        
        public SyntaxTree(Errors errors, BlockStatement root, SyntaxToken end)
        {
            Errs = errors;
            Root = root;
            End = end;
        }
    }
}