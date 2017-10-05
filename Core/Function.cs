using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    class Function : Identifier
    {
        public Function(
            string name, 
            Type type, 
            List<Parameter> args
        ) : base(name, type)
        {
            Parameters = args;
        }

        public List<Parameter> Parameters { get; internal set; }
    }
}
