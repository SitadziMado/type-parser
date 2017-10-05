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

        public override string ToString()
        {
            string pass =   (Pass == PassType.Ref) ? "ByRef" :
                            ((Pass == PassType.Out) ? "Out" : "ByVal");
            var sb = new StringBuilder();
            sb.AppendFormat(
                "{0} параметр:\tИмя: {1}\tТип: {2}\tЗначение: {3}\t",
                pass,
                Name,
                Typeid,
                Value
            );
            return sb.ToString();
        }

        public PassType Pass { get; internal set; }
    }
}