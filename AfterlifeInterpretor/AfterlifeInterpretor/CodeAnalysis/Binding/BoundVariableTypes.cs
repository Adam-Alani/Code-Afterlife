using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundVariableTypes
    {
        public static Type GetType(SyntaxToken token)
        {
            return token.Kind switch
            {
                SyntaxKind.VarKeyword => typeof(object),
                SyntaxKind.BoolKeyword => typeof(bool),
                SyntaxKind.IntKeyword => typeof(int),
                SyntaxKind.FloatKeyword => typeof(double),
                SyntaxKind.StringKeyword => typeof(string),
                SyntaxKind.ListKeyword => typeof(List),
                SyntaxKind.FunctionKeyword => typeof(Function),
                _ => null
            };
        }
    }
}