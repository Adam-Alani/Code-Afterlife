using System;
using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis
{
    internal sealed class Scope
    {
        public Dictionary<string, object> Variables { get; }
        public Scope Parent;
        public bool Return;
        private bool _allowChanges;
        public Scope Caller { get; set; }
        
        public Scope()
        {
            Parent = null;
            Variables = new Dictionary<string, object>();
            Return = false;
            _allowChanges = true;
            Caller = null;
        }
        
        public Scope(Dictionary<string, object> variables)
        {
            Parent = null;
            Variables = variables;
            Return = false;
            _allowChanges = true;
        }
        public Scope(Scope parent)
        {
            Parent = parent;
            Variables = new Dictionary<string, object>();
            Return = false;
            _allowChanges = true;
        }
        public Scope(Scope parent, Dictionary<string, object> variables)
        {
            Parent = parent;
            Variables = variables;
            Return = false;
            _allowChanges = true;
        }

        public bool HasVariable(string var)
        {
            return  _allowChanges && (Variables.ContainsKey(var) || (Parent != null && Parent.HasVariable(var)));
        }

        public void Declare(string var, object val)
        {
            if (_allowChanges &&!HasVariable(var))
                Variables.Add(var, val);
        }

        public void Undeclare(string var)
        {
            if (_allowChanges && !Variables.Remove(var)&& Parent != null)
                Parent.Undeclare(var);
        }

        public void SetValue(string var, object val)
        {
            if (_allowChanges)
            {
                if (Variables.ContainsKey(var))
                    Variables[var] = val;
                else 
                    Parent?.SetValue(var, val);
            }
        }

        public object GetValue(string var)
        {
            if (Variables.ContainsKey(var))
                return Variables[var];
            return Parent?.GetValue(var);
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

        public Dictionary<string, Function> GetFunctions()
        {
            Dictionary<string, Function> functions = new Dictionary<string, Function>();
            foreach (KeyValuePair<string, object> kv in Variables)
            {
                if (kv.Value is Function f)
                    functions.Add(kv.Key, f);
            }

            return functions;
        }

        public void ForbidChanges()
        {
            _allowChanges = false;
        }

        public void AllowChanges()
        {
            _allowChanges = true;
        }
    }
}