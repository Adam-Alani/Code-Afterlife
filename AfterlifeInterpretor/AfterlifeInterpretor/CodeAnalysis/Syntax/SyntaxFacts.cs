namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    /// <summary>
    /// SyntaxFact class
    /// Static class that can be used to get information on how data is supposed to be parsed (i.e.: precedence etc.)
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal static class SyntaxFacts
    {
        public static int GetUnaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.NotToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 6;
                default:
                    return 0;
            }
        }
        
        public static int GetBinaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.OrToken:
                    return 1;
                case SyntaxKind.AndToken:
                    return 2;
                case SyntaxKind.EqToken:
                case SyntaxKind.NEqToken:
                case SyntaxKind.GtEqToken:
                case SyntaxKind.LtEqToken:
                case SyntaxKind.GtToken:
                case SyntaxKind.LtToken:
                    return 3;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.ModuloToken:
                    return 5;
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            return text switch
            {
                "true" => SyntaxKind.TrueKeyword,
                "false" => SyntaxKind.FalseKeyword,
                "and" => SyntaxKind.AndToken,
                "or" => SyntaxKind.OrToken,
                "not" => SyntaxKind.NotToken,
                "bool" => SyntaxKind.BoolToken,
                "int" => SyntaxKind.IntToken,
                "var" => SyntaxKind.VarToken,
                "do" => SyntaxKind.OStatementToken,
                "end" => SyntaxKind.CStatementToken,
                _ => SyntaxKind.IdentifierToken
            };
        }
        
        public static SyntaxKind GetSymbolKind(string s)
        {
            return s switch
            {
                "\0" => SyntaxKind.EndToken,
                "+" => SyntaxKind.PlusToken,
                "-" => SyntaxKind.MinusToken,
                "*" => SyntaxKind.StarToken,
                "/" => SyntaxKind.SlashToken,
                "%" => SyntaxKind.ModuloToken,
                "(" => SyntaxKind.OParenToken,
                ")" => SyntaxKind.CParenToken,
                "{" => SyntaxKind.OStatementToken,
                "}" => SyntaxKind.CStatementToken,
                ">" => SyntaxKind.GtToken,
                "<" => SyntaxKind.LtToken,
                "=" => SyntaxKind.AssignToken,
                "!" => SyntaxKind.NotToken,
                "||" => SyntaxKind.OrToken,
                "&&" => SyntaxKind.AndToken,
                "==" => SyntaxKind.EqToken,
                "!=" => SyntaxKind.NEqToken,
                ">=" => SyntaxKind.GtEqToken,
                "<=" => SyntaxKind.LtEqToken,
                _ => SyntaxKind.ErrorToken
            };
        }

        public static bool ExpectedLookahead(char current, char lookAhead)
        {
            switch (current)
            {
                case '\0':
                    return false;
                case '!':
                case '>':
                case '<':
                    return lookAhead == '=';
                default:
                    return lookAhead == current;
            }
        }
    }
}