using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeParser
{
    public abstract class Identifier : IComparable<Identifier>
    {
        public Identifier(string name, Type type)
        {
            Name = name;
            Typeid = type;
        }

        public enum TypeId
        {
            None,
            Variable,
            Function,
            Class,
        }

        public enum PassType
        {
            ByVal,
            Ref,
            Out,
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Name.Equals((obj as Identifier).Name);
        }

        public override int GetHashCode()
        {
            return new { Name }.GetHashCode();
        }

        public int CompareTo(Identifier other)
        {
            return Name.CompareTo(other.Name);
        }

        public string Name { get; internal set; }
        public Type Typeid { get; internal set; }
    }
}
