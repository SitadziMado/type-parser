﻿using System;
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

            BinaryTree<Identifier> tree = null;

            try
            {
                tree = (
                    from v
                    in source.Split(';')
                    where v.Length > 0
                    select Identifier.Factory.FromString(v.Trim())
                ).ToBinaryTree();
            }
            catch (ParserException e)
            {
                Console.WriteLine(
                    String.Format(
                        "Исходный файл содержит ошибку в строке {0}.", 
                        tree?.Count + 2
                    )
                );
                return;
            }

            Console.WriteLine("Исходный файл преобразован в двоичное дерево.");
        }
    }
}
