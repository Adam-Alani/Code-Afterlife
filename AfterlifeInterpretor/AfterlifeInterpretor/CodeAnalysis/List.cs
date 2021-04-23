using System;
using System.Collections;

namespace AfterlifeInterpretor.CodeAnalysis
{
    internal class List : IEnumerable
    {
        public object Head { get; }
        public List Tail { get; }
        
        public int Size { get; }

        public bool IsEmpty { get; }


        public List()
        {
            Head = null;
            Tail = null;
            Size = 0;
            IsEmpty = true;
        }
        
        public List(object head, List tail = null)
        {
            Head = head;
            Tail = tail ?? new List();
            IsEmpty = false;
            Size = 1 + (tail?.Size ?? 0);
        }

        public override bool Equals(object? obj)
        {
            if (obj is List l)
                return Size == l.Size && Equals(Head, l.Head) && Equals(Tail, l.Tail);
            return false;
        }

        protected bool Equals(List other)
        {
            return Equals(Head, other.Head) && Equals(Tail, other.Tail) && Size == other.Size && IsEmpty == other.IsEmpty;
        }
        public static List operator +(List l1, List l2)
        {
            if (l1.IsEmpty)
                return l2;
            if (l2.IsEmpty)
                return l1;
            return l1.Tail == null ? new List(l1.Head, l2) : new List(l1.Head, l1.Tail + l2);
        }
        
        public static List operator +(List l1, object obj)
        {
            if (obj is List l2)
                return l1 + l2;

            if (l1.IsEmpty)
                return new List(obj);
            
            return l1.Tail == null ? new List(l1.Head, new List(obj)) : new List(l1.Head, l1.Tail + obj);
        }
        
        public static List operator +(object obj, List l1)
        {
            if (obj is List l2)
                return l2 + l1;

            if (l1.IsEmpty)
                return new List(obj);
            
            return new List(obj, l1);
        }

        public override string ToString()
        {
            if (IsEmpty)
                return "<EmptyList>";
            
            string tail = (Tail == null) ? "" : Tail.ToString();
            string head = Head?.ToString();
            if (Head is List)
                head = "(" + head + ")";
            if (Head is string)
                head = '"' + head + '"';
            if (Head is null)
                head = "()";
            return head + ", " + tail;
        }

        public IEnumerator GetEnumerator()
        {
            return new ListEnumerator(this);
        }

        public object this[int i] => AtIndex(i);

        private object AtIndex(int n)
        {
            if (Tail == null && n > 0)
                throw new IndexOutOfRangeException();

            return n == 0 ? Head : Tail[n - 1];
        }
    }

    internal sealed class ListEnumerator : IEnumerator
    {
        private readonly List _list;
        private List _current;

        private object? Current
        {
            get
            {
                if (_current != null)
                    return _current.Head;
                throw new IndexOutOfRangeException();
            }
        }

        object IEnumerator.Current => Current;

        public ListEnumerator(List list)
        {
            _list = list;
            _current = _list;
        }
        
        public bool MoveNext()
        {
            if (_current != null)
            {
                _current = _current.Tail;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = _list;
        }
    }
}