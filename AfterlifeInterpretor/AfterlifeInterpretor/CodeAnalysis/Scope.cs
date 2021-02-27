using System;
using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis
{
    internal sealed class Scope
    {
        public Dictionary<string, object> Variables { get; }
        private Scope _parent;

        public Scope()
        {
            _parent = null;
            Variables = new Dictionary<string, object>();
        }
        
        public Scope(Dictionary<string, object> variables)
        {
            _parent = null;
            Variables = variables;
        }
        public Scope(Scope parent)
        {
            _parent = parent;
            Variables = new Dictionary<string, object>();
        }
        public Scope(Scope parent, Dictionary<string, object> variables)
        {
            _parent = parent;
            Variables = variables;
        }

        public bool HasVariable(string var)
        {
            return Variables.ContainsKey(var) || (_parent != null && _parent.HasVariable(var));
        }

        public void Declare(string var, object val)
        {
            if (!Variables.ContainsKey(var))
                Variables.Add(var, val);
        }

        public void SetValue(string var, object val)
        {
            if (Variables.ContainsKey(var))
                Variables[var] = val;
            else
                _parent?.SetValue(var, val);
        }

        public object GetValue(string var)
        {
            if (Variables.ContainsKey(var))
                return Variables[var];
            return _parent?.GetValue(var);
        }

        public Dictionary<string, Type> GetTypes()
        {
            Dictionary<string, Type> res = new Dictionary<string, Type>();
            foreach (KeyValuePair<string, object> kv in Variables)
            {
                
                res.Add(kv.Key, kv.Value != null ? kv.Value.GetType() : typeof(object));
            }

            return res;
        }
    }
}