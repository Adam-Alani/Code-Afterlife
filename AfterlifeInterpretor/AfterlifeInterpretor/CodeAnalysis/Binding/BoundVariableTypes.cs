using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundVariableTypes
    {
        public static Type GetType(SyntaxToken token)
        {
            return token.Kind switch
            {
                SyntaxKind.VarToken => typeof(object),
                SyntaxKind.BoolToken => typeof(bool),
                SyntaxKind.IntToken => typeof(int),
                _ => null
            };
        }
    }
}