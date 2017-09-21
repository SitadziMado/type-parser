using System;

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
    }
}