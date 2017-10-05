using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    class Variable : Identifier
    {
        public Variable(string name, Type type, object value) : base(name, type)
        {
            Value = value;
            IsConst = false;
        }

        public object Value { get; set; }
        public bool IsConst { get; internal set; }
    }
}
