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

        public override string ToString()
        {
            var paramSB = new StringBuilder();

            foreach (var v in Parameters)
                paramSB.AppendFormat(Environment.NewLine + "\t{0}", v);

            var sb = new StringBuilder();
            sb.AppendFormat(
                "Функция:\tИмя: {0}\tТип: {1}\tПараметры: {2}",
                Name,
                Typeid,
                paramSB
            );
            return sb.ToString();
        }

        public List<Parameter> Parameters { get; internal set; }
    }
}
