using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    class Class : Identifier
    {
        public Class(string name, Type type) : base(name, type)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                "Класс {0}",
                Name
            );
            return sb.ToString();
        }
    }
}
