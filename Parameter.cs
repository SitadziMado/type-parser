using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    class Parameter : Variable
    {
        public Parameter(
            string name,
            Type type, 
            object value, 
            PassType pass
        ) : base(name, type, value)
        {
            Pass = pass;
        }

        public Parameter(
            Variable other,
            PassType pass
        ) : base(other.Name, other.Typeid, other.Value)
        {
            Pass = pass;
        }

        public PassType Pass { get; internal set; }
    }
}