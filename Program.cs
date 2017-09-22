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
            string source = 
@"int a;
const float c = 10;
class MyClass;
string method1(int x1, ref char x2, out float x3);";

            var req = (
                from v
                in source.Split(';')
                where v.Length > 0
                select Identifier.Factory.FromString(v.Trim())
            ).ToArray();
        }
    }
}
