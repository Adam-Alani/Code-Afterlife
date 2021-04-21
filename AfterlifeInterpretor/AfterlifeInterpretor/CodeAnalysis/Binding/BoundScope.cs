using System;
using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {
        public BoundScope Parent;
        private Dictionary<string, Type> Variables { get; }

        public Type BlockType;

        
        public BoundScope()
        {
            Parent = null;
            Variables = new Dictionary<string, Type>();
            BlockType = null;
        }
        
        public BoundScope(BoundScope parent)
        {
            Parent = parent;
            Variables = new Dictionary<string, Type>();
            BlockType = parent.BlockType;
        }
        
        public BoundScope(Dictionary<string, Type> variables)
        {
            Parent = null;
            Variables = variables;
            BlockType = null;

        }

        public BoundScope(BoundScope parent, Dictionary<string, Type> variables)
        {
            Parent = parent;
            Variables = variables;
            BlockType = parent.BlockType;
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
    }
}