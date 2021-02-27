using System;
using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis
{
    public sealed class Error
    {
        public string Message { get; }
        public int Position { get; }

        public Error(string message, int position)
        {
            Message = message;
            Position = position;
        }

        public string ToString(int line, int pos)
        {
            return $"({line}, {pos}): {Message}";
        }
        
        public string ToString(int line)
        {
            return $"({line}, {Position}): {Message}";
        }
        
        public string ToString(string text)
        {
            Text.GetIndex(text, Position,out int line, out int pos);
            return $"({line}, {pos}): {Message}";
        }
        
        public override string ToString()
        {
            return $"({Position}): {Message}";
        }
    }
    
    public sealed class Errors
    {
        private List<Error> _errors;
        
        public Errors(List<Error> errors)
        {
            _errors = errors;
        }
        
        public Errors()
        {
            _errors = new List<Error>();
        }

        public List<Error> GetErrors()
        {
            return _errors;
        }

        public void Report(string message, int position)
        {
            _errors.Add(new Error(message, position));
        }
        
        public void ReportUnknown(object value, int position)
        {
            Report($"Unknown value '{value}'", position);
        }

        public void ReportUnexpected(object expected, object got, int position)
        {
            Report($"Expected {expected}, got {got}", position);
        }

        public void ReportUndefined(object o, Type t, int position)
        {
            Report($"Operation '{o}' is not defined for {t}", position);
        }

        public void ReportType(Type a, Type b, int position)
        {
            Report($"Invalid operation between {a} and {b}", position);
        }
        
        public void ReportType(object op, Type a, Type b, int position)
        {
            Report($"Invalid operation '{op}' between {a} and {b}", position);
        }
        

        public void ReportDeclared(string name, int position)
        {
            Report($"Variable '{name}' has already been declared", position);
        }
    }
}