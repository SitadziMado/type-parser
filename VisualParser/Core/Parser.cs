using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeParser;

namespace VisualParser.Core
{
    /// <summary>
    /// Класс для парсинга кода на C#
    /// </summary>
    public class IdentifierParser
    {
        public BinaryTree<Identifier> FromString(string code)
        {
            BinaryTree<Identifier> tree = new BinaryTree<Identifier>();

            var req = from v
                      in code.Split(';')
                      let w = v.Trim()
                      where w.Length > 0
                      select w;

            foreach (var u in req)
            {
                Queue<string> tokens = new Queue<string>(
                    from v
                    in Regex.Split(u.Trim().Replace("()", "(void)"), @"\b")
                    let w = v.Trim()
                    where w.Length > 0
                    select w
                );

                tree.Add(ParseIdentifier(tokens));
            }

            return tree;
        }

        private Identifier ParseIdentifier(Queue<string> tokens)
        {
            Identifier result;
            string cur = String.Empty;

            string Move()
            {
                var t = cur;

                try
                {
                    cur = tokens.Dequeue();
                }
                catch (InvalidOperationException)
                {
                    cur = string.Empty;
                }

                return t;
            }

            Move();

            if (cur == "class")
            {
                // Возвращаем класс
                result = new Class(Move(), typeof(object));
            }
            else
            {
                string pass = string.Empty;
                bool isConst = false;

                if (cur == "const")
                {
                    isConst = Move() == "const";
                }
                else if (cur == "ref" || cur == "out")
                {
                    pass = Move();
                }

                var type = mTypes[Move()];
                var name = Move();

                if (cur == "(") // Далее парсим функцию
                {   
                    result = new Function(name, type, ParseParameters(tokens));
                }
                else if (cur == "()")
                {
                    result = new Function(name, type, new List<Parameter>());
                }
                else // Парсим переменную, константу или параметр
                {   
                    object value = null;

                    if (cur != string.Empty && cur[0] == '=')
                    {
                        value = ParseValue(tokens, type);
                    }

                    result = isConst 
                        ? new Constant(name, type, value)
                        : result = new Variable(name, type, value);
                    
                    if (pass != string.Empty)
                    {
                        result = new Parameter((Variable)result, PassTypeFromString(pass));
                    }
                }
            }

            return result;
        }

        private Identifier.PassType PassTypeFromString(string pt)
        {
            Identifier.PassType result = Identifier.PassType.ByVal;

            switch (pt)
            {
                case "ref": result = Identifier.PassType.Ref; break;
                case "out": result = Identifier.PassType.Out; break;
                default: break;
            }

            return result;
        }

        private object ParseValue(Queue<string> tokens, Type type)
        {
            var sb = new StringBuilder();

            while (tokens.Count > 0)
            {
                sb.Append(tokens.Dequeue());
            }

            return Convert.ChangeType(
                sb.ToString()
                    .Replace('.', ',')
                    .Replace("'", "")
                    .Replace("\"", "").Trim(),
                type
            );
        }

        private List<Parameter> ParseParameters(Queue<string> tokens)
        {
            List<Parameter> args = new List<Parameter>();
            Queue<string> param = new Queue<string>();

            string cur = string.Empty;

            while ((cur = tokens.Dequeue()) != ")")
            {
                if (cur != ",")
                {
                    param.Enqueue(cur);
                }
                else
                {
                    var next = ParseIdentifier(param);
                    Parameter p;

                    if (next is Variable)
                    {
                        p = new Parameter((Variable)next, Identifier.PassType.ByVal);
                    }
                    else
                    {
                        p = (Parameter)next;
                    }

                    args.Add(p);
                    param.Clear();
                }
            }

            return args;
        }

        private static readonly Dictionary<string, Type> mTypes = new Dictionary<string, Type>()
        {
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) },
            { "ushort", typeof(ushort) },
            { "short", typeof(short) },
            { "uint", typeof(uint) },
            { "int", typeof(int) },
            { "ulong", typeof(ulong) },
            { "long", typeof(long) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "decimal", typeof(decimal) },
            { "char", typeof(char) },
            { "string", typeof(string) },
            { "bool", typeof(bool) },
        };
    }
}
