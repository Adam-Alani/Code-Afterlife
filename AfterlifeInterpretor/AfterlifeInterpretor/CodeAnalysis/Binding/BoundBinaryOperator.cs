using System;
using System.Globalization;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryKind kind, Type leftType, Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }
        
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

        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.OrToken, BoundBinaryKind.Or, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.AndToken, BoundBinaryKind.And, typeof(bool)), 
            
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryKind.Add, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryKind.Sub, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryKind.Mul, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryKind.Div, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.ModuloToken, BoundBinaryKind.Mod, typeof(int)), 
            new BoundBinaryOperator(SyntaxKind.GtToken, BoundBinaryKind.Gt, typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtToken, BoundBinaryKind.Lt, typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.GtEqToken, BoundBinaryKind.GtEq, typeof(int), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LtEqToken, BoundBinaryKind.LtEq, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(int),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(int), typeof(bool)),
            
            new BoundBinaryOperator(SyntaxKind.EqToken, BoundBinaryKind.Eq, typeof(bool),  typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.NEqToken, BoundBinaryKind.Neq, typeof(bool), typeof(bool)),
        };

        public static BoundBinaryOperator Bind(SyntaxKind kind, Type leftType, Type rightType)
        {
            foreach (BoundBinaryOperator op in _operators)
            {
                if (op.SyntaxKind == kind && op.LeftType == leftType && op.RightType == rightType)
                    return op;
            }

            return null;
        }
    }
}