using System;
using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {
        public BoundScope Parent;
        private Dictionary<string, Type> Variables { get; }
        
        private Dictionary<string, Function> Functions { get; }

        public Type BlockType;

        public string TypeString;

        
        public BoundScope(BoundScope parent = null, Dictionary<string, Type> variables = null, Dictionary<string, Function> functions = null)
        {
            Parent = parent;
            Variables = variables ?? new Dictionary<string, Type>();
            BlockType = parent?.BlockType;
            Functions = functions ?? new Dictionary<string, Function>();
            TypeString = null;
        }

        public bool HasVariable(string var)
        {
            return Variables.ContainsKey(var) || (Parent?.HasVariable(var) ?? false);
        }

        public bool TryDeclare(string var, Type type)
        {
            if (!HasVariable(var))
            {
                Variables.Add(var, type);
                if (type == typeof(Function))
                    Functions.Add(var, null);
                return true;
            }

            return false;
        }

        public bool TryGetType(string var, out Type t)
        {
            if (Variables.ContainsKey(var))
            {
                t = Variables[var];
                return true;
            }
            
            t = null;
            return Parent?.TryGetType(var, out t) ?? false;
        }

        public void ChangeType(string var, Type t)
        {
            if (Variables.ContainsKey(var))
                Variables[var] = t;
            else
                Parent?.ChangeType(var, t);
        }

        public void SetFunction(string var, Type t)
        {
            if (Functions.ContainsKey(var))
            {
                Functions[var] = new Function(null, null, t, null);
            }
        }

        public Function GetFunction(string name)
        {
            if (Functions.ContainsKey(name))
                return Functions[name];
            return Parent?.GetFunction(name);
        }
    }
}