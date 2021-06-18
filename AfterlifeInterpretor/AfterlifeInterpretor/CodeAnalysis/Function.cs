using System;
using AfterlifeInterpretor.CodeAnalysis.Binding;

namespace AfterlifeInterpretor.CodeAnalysis
{
    internal class Function
    {
        public BoundExpression Args { get; }
        public BoundStatement Body { get; }
        
        public Type Type { get; }
        
        public string TypeString { get; }
        
        public Scope Scope { get; }

        public Function(BoundExpression args, BoundStatement body, Type type, Scope scope, string typeString = null)
        {
            Args = args;
            Body = body;
            Type = type;
            Scope = scope;
            TypeString = typeString ?? ((Type != null) ? Text.PrettyType(Type) : "()");
        }

        public override string ToString()
        {
            string type = TypeString;
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

        public string GetTypeDepth(int depth)
        {
            if (depth <= 0)
                return ToString();
            
            if (Body is BoundExpressionStatement {Expression: BoundFunction bf})
            {
                return (new Function(bf.Args, bf.Body, Type, Scope)).GetTypeDepth(depth - 1);
            }

            if (depth > 1)
                return "";

            return Text.PrettyType(Type);
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
        
        
        private static string GetCallType(BoundCallExpression bce, BoundScope scope)
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
                return scope.GetFunction(bv.Name).GetTypeDepth(depth);
            }
            return "function";
        }

        private string GetArgString(BoundExpression arg)
        {
            if (arg is BoundEmptyListExpression)
                return "()";
            if (arg is BoundBinary bb)
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