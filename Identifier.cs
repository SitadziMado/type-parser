using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TypeParser
{
    abstract class Identifier
    {
        public Identifier(string name, TypeId type)
        {
            Name = name;
            Type = type;
        }

        public enum TypeId
        {
            None,
            Variable,
            Function,
            Class,
        }

        public static class Factory
        {
            public static Identifier FromString(string code)
            {
                var tokens = (
                    from v
                    in Regex.Split(code, SplitRegex)
                    where v.Length > 0
                    select v
                );

                throw new NotImplementedException(); 
            }

            private const string SplitRegex = @"\b";
        }
        
        public string Name { get; set; }
        public TypeId Type { get; set; }
    }
}
