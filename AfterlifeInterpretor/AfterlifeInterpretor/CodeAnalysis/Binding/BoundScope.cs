using System;
using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {
        private BoundScope _parent;
        private Dictionary<string, Type> Variables { get; }

        
        public BoundScope()
        {
            _parent = null;
            Variables = new Dictionary<string, Type>();
        }
        
        public BoundScope(BoundScope parent)
        {
            _parent = parent;
            Variables = new Dictionary<string, Type>();
        }
        
        public BoundScope(Dictionary<string, Type> variables)
        {
            _parent = null;
            Variables = variables;
        }

        public BoundScope(BoundScope parent, Dictionary<string, Type> variables)
        {
            _parent = parent;
            Variables = variables;
        }
        
        public bool HasVariable(string var)
        {
            return Variables.ContainsKey(var) || (_parent?.HasVariable(var) ?? false);
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
            return _parent?.TryGetType(var, out t) ?? false;
        }
    }
}