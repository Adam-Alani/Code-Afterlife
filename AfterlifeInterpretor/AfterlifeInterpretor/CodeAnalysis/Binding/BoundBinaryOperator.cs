using System;
using System.Linq;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }
        
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryKind kind, Type type)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = type;
            RightType = type;
            ResultType = type;
        }
        
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryKind kind, Type operandsType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = operandsType;
            RightType = operandsType;
            ResultType = resultType;
        }
        
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryKind kind, Type leftType, Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.OrToken, BoundBinaryKind.Or, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.AndToken, BoundBinaryKind.And, typeof(bool)), 
            
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(double), typeof(int), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryKind.Sub, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryKind.Sub, typeof(double), typeof(int), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryKind.Mul, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryKind.Mul, typeof(double), typeof(int), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryKind.Div, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryKind.Div, typeof(double), typeof(int), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.DoubleSlashToken, BoundBinaryKind.IntDiv, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.DoubleSlashToken, BoundBinaryKind.IntDiv, typeof(int), typeof(double), typeof(int)),
            new BoundBinaryOperator(SyntaxKind.ModuloToken, BoundBinaryKind.Mod, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.PlusAssignToken, BoundBinaryKind.Add, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.MinusAssignToken, BoundBinaryKind.Sub, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.StarAssignToken, BoundBinaryKind.Mul, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.SlashAssignToken, BoundBinaryKind.Div, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.ModuloAssignToken, BoundBinaryKind.Mod, typeof(int)), 
            
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(int), typeof(double), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryKind.Sub, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryKind.Sub, typeof(int), typeof(double), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryKind.Mul, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryKind.Mul, typeof(int), typeof(double), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryKind.Div, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryKind.Div, typeof(int), typeof(double), typeof(double)),
            new BoundBinaryOperator(SyntaxKind.DoubleSlashToken, BoundBinaryKind.IntDiv, typeof(double), typeof(int)),
            new BoundBinaryOperator(SyntaxKind.DoubleSlashToken, BoundBinaryKind.IntDiv, typeof(double), typeof(int), typeof(int)),
            new BoundBinaryOperator(SyntaxKind.PlusAssignToken, BoundBinaryKind.Add, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.MinusAssignToken, BoundBinaryKind.Sub, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.StarAssignToken, BoundBinaryKind.Mul, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.SlashAssignToken, BoundBinaryKind.Div, typeof(double)), 
            new BoundBinaryOperator(SyntaxKind.ModuloAssignToken, BoundBinaryKind.Mod, typeof(double)), 
            
            new BoundBinaryOperator(SyntaxKind.GtToken, BoundBinaryKind.Gt, typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtToken, BoundBinaryKind.Lt, typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.GtEqToken, BoundBinaryKind.GtEq, typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtEqToken, BoundBinaryKind.LtEq, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(int),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(int), typeof(bool)),
            
            new BoundBinaryOperator(SyntaxKind.GtToken, BoundBinaryKind.Gt, typeof(int), typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtToken, BoundBinaryKind.Lt, typeof(int), typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.GtEqToken, BoundBinaryKind.GtEq, typeof(int), typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtEqToken, BoundBinaryKind.LtEq, typeof(int), typeof(double), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(int), typeof(double),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(int), typeof(double), typeof(bool)),
            
            new BoundBinaryOperator(SyntaxKind.GtToken, BoundBinaryKind.Gt, typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtToken, BoundBinaryKind.Lt, typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.GtEqToken, BoundBinaryKind.GtEq, typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtEqToken, BoundBinaryKind.LtEq, typeof(double), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(double),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(double), typeof(bool)),
            
            
            new BoundBinaryOperator(SyntaxKind.GtToken, BoundBinaryKind.Gt, typeof(double), typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtToken, BoundBinaryKind.Lt, typeof(double), typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.GtEqToken, BoundBinaryKind.GtEq, typeof(double), typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtEqToken, BoundBinaryKind.LtEq, typeof(double), typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(double), typeof(int),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(double), typeof(int), typeof(bool)),
            
            new BoundBinaryOperator(SyntaxKind.DotToken, BoundBinaryKind.Dot, typeof(string), typeof(int), typeof(string)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryKind.Mul, typeof(int), typeof(string), typeof(string)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryKind.Mul, typeof(string), typeof(int), typeof(string)),
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(string), typeof(object), typeof(string)), 
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(object), typeof(string), typeof(string)),
            new BoundBinaryOperator(SyntaxKind.PlusAssignToken, BoundBinaryKind.Add, typeof(string)), 
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(string),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(string), typeof(bool)),
            
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(bool),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(bool), typeof(bool)),
            
            new BoundBinaryOperator(SyntaxKind.CommaToken, BoundBinaryKind.Comma, typeof(object), typeof(List)),
            new BoundBinaryOperator(SyntaxKind.DotToken, BoundBinaryKind.Dot, typeof(List), typeof(int), typeof(Unpredictable)),
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(List)),
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(List), typeof(object), typeof(List)),
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(object), typeof(List), typeof(List)),
            new BoundBinaryOperator(SyntaxKind.PlusAssignToken, BoundBinaryKind.Add, typeof(List)),
            new BoundBinaryOperator(SyntaxKind.PlusAssignToken, BoundBinaryKind.Add, typeof(List), typeof(object), typeof(List)),
            new BoundBinaryOperator(SyntaxKind.PlusAssignToken, BoundBinaryKind.Add, typeof(object), typeof(List), typeof(List)),
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(List),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(List), typeof(bool)),
        };

        public static BoundBinaryOperator Bind(SyntaxKind kind, Type leftType, Type rightType)
        {
            return _operators.FirstOrDefault(op => op.SyntaxKind == kind && (op.LeftType == leftType || op.LeftType == typeof(object) || leftType == typeof(Unpredictable)) && (op.RightType == rightType || op.RightType == typeof(object) || rightType == typeof(Unpredictable)));
        }

        private static Type GetHarmonisation(Tuple<Type, Type> types)
        {
            if (types.Item1 == typeof(string) || types.Item2 == typeof(string))
                return typeof(string);
            if (types.Item1 == typeof(double) && types.Item2 == typeof(int) || types.Item1 == typeof(int) && types.Item2 == typeof(double))
                return typeof(double);

            return null;
        }

        public static void HarmoniseTypes(ref Type l, ref Type r)
        {
            Type t = GetHarmonisation(new Tuple<Type, Type>(l, r));
            if (t != null)
            {
                l = t;
                r = t;
            }
        }
    }
}