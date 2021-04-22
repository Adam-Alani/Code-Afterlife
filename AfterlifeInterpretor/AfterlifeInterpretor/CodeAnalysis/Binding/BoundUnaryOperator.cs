using System;
using System.Linq;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {
        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }
        
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryKind kind, Type operandType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = operandType;
        }
        
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryKind kind, Type operandType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.NotToken, BoundUnaryKind.Not, typeof(bool)), 
            
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryKind.Id, typeof(int)), 
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryKind.Neg, typeof(int)),
            
            new BoundUnaryOperator(SyntaxKind.HeadToken, BoundUnaryKind.Head, typeof(List),typeof(Unpredictable)),
            new BoundUnaryOperator(SyntaxKind.TailToken, BoundUnaryKind.Tail, typeof(List)),
            new BoundUnaryOperator(SyntaxKind.SizeToken, BoundUnaryKind.Size, typeof(List), typeof(int)),
            
            new BoundUnaryOperator(SyntaxKind.HeadToken, BoundUnaryKind.Head, typeof(string),typeof(string)),
            new BoundUnaryOperator(SyntaxKind.TailToken, BoundUnaryKind.Tail, typeof(string)),
            new BoundUnaryOperator(SyntaxKind.SizeToken, BoundUnaryKind.Size, typeof(string), typeof(int)),
            
            new BoundUnaryOperator(SyntaxKind.PrintToken, BoundUnaryKind.Print, typeof(object), typeof(string))

        };

        public static BoundUnaryOperator Bind(SyntaxKind kind, Type type)
        {
            return _operators.FirstOrDefault(op => op.SyntaxKind == kind && op.OperandType == type || op.OperandType == typeof(object));
        }
    }
}