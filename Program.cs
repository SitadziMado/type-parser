using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = Type.GetType("System.Int32");
            var ident = Identifier.Factory.FromString("string method1(int x1, ref char x2, out float x3)");
        }
    }
}
