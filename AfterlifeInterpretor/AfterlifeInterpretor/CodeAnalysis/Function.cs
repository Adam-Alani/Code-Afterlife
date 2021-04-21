using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AfterlifeInterpretor.CodeAnalysis.Binding;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Parser;
using Binder = System.Reflection.Binder;

namespace AfterlifeInterpretor.CodeAnalysis
{
    internal class Function
    {
        public BoundExpression Args { get; }
        public BoundStatement Body { get; }
        
        public Type Type { get; private set; }
        
        public Scope Scope { get; }

        public Function(BoundExpression args, BoundStatement body, Type type, Scope scope)
        {
            Args = args;
            Body = body;
            Type = type;
            Scope = scope;
        }

        public override string ToString()
        {
            string type = (Type != null) ? Text.PrettyType(Type) : "()";
            if (Body is BoundExpressionStatement {Expression: BoundCallExpression bce})
            {
                type = GetCallType(bce);
            }
            if (Body is BoundExpressionStatement {Expression: BoundFunction bf})
            {
                type = (new Function(bf.Args, bf.Body, Type, Scope)).ToString();
            }
            if (Body is BoundExpressionStatement {Expression: BoundVariable bv} && bv.Type == typeof(Function))
            {
                type = "(" + Scope.GetValue(bv.Name) + ")";
            }
            return $"{GetArgString(Args)} -> {type}";
        }

        private string GetTypeDepth(int depth)
        {
            if (depth <= 0)
                return ToString();
            if (Body is BoundExpressionStatement {Expression: BoundFunction bf})
            {
                return (new Function(bf.Args, bf.Body, Type, Scope)).GetTypeDepth(depth - 1);
            }

            return "";
        }

        private string GetCallType(BoundCallExpression bce)
        {
            int depth = 0;
            while (bce.Called is BoundCallExpression bce1)
            {
                depth++;
                bce = bce1;
            }

            if (!(bce.Args is BoundEmptyListExpression))
                depth++;

            if (bce.Called is BoundVariable bv)
            {
                object v = Scope.GetValue(bv.Name);
                if (v is Function f)
                {
                    return f.GetTypeDepth(depth);
                }
            }
            return "function";
        }

        private string GetArgString(BoundExpression arg)
        {
            if (Args is BoundEmptyListExpression)
                return "()";
            if (Args is BoundBinary bb)
            {
                string left = (bb.Left is BoundBinary bbl) ? "(" + GetArgString(bbl) + ")" : (bb.Left is BoundVariable) ? Text.PrettyType(bb.Left.Type) : bb.Left.ToString();
                string right = (bb.Right is BoundBinary bbr) ? GetArgString(bbr) :(bb.Right is BoundVariable) ? Text.PrettyType(bb.Right.Type) : bb.Right.ToString();
            
                if (right == "()")
                    return left;
                return left + ", " + right;
            }

            return "";
        }
    }
}