using System;
using System.Text;

namespace TypeParser
{
    class Constant : Variable
    {
        public Constant(
            string name,
            Type type,
            object value
        ) : base(name, type, value)
        {
            IsConst = true;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                "\rКонстанта:\tИмя: {0}\tТип: {1}\tЗначение: {2}",
                Name,
                Typeid,
                Value
            );
            return sb.ToString();
        }
    }
}