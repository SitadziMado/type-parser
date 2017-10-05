using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TypeParser
{
    abstract class Identifier : IComparable<Identifier>
    {
        public Identifier(string name, Type type)
        {
            Name = name;
            Typeid = type;
        }

        public enum TypeId
        {
            None,
            Variable,
            Function,
            Class,
        }

        public enum PassType
        {
            ByVal,
            Ref,
            Out,
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Name.Equals((obj as Identifier).Name);
        }

        public override int GetHashCode()
        {
            return new { Name }.GetHashCode();
        }

        public int CompareTo(Identifier other)
        {
            return Name.CompareTo(other.Name);
        }

        public static class Factory
        {
            public static Identifier FromString(string code)
            {
                code = code.Replace("()", "(void)");

                Queue<string> tokens = new Queue<string>(
                    from v
                    in Regex.Split(code.Trim(), SplitRegex)
                    where v.Trim().Length > 0
                    select v.Trim()
                );
                
                bool isConst = false;

                var first = tokens.Dequeue();

                if (first == "const")
                {
                    isConst = true;
                    first = tokens.Dequeue();
                }
                else if (first == "class")
                {
                    return new Class(tokens.Dequeue(), typeof(object));
                }
                else if (first == "void")
                {
                    return new Variable("__void", typeof(void), null);
                }
                
                try
                {
                    // Type type = Type.GetType(first + ", TypeParser", true, false);
                    Type type = mTypes[first];
                    string identifier = tokens.Dequeue();
                    object value = null;
                    var parameters = new List<Parameter>();

                    if (tokens.Count > 1)
                    {
                        string operation = tokens.Dequeue();

                        if (operation[0] == '=')
                        {
                            var sb = new StringBuilder(operation.Substring(1));

                            while (tokens.Count > 0)
                                sb.Append(tokens.Dequeue());

                            value = Convert.ChangeType(
                                sb.ToString()
                                    .Replace('.', ',')
                                    .Replace("'", "")
                                    .Replace("\"", "").Trim(),
                                type
                            );

                            if (isConst)
                                return new Constant(identifier, type, value);
                            else
                                return new Variable(identifier, type, value);
                        }
                        else if (operation == "(")
                        {
                            #region Функция
                            // operation = tokens.Dequeue();

                            while (operation != ")")
                            {
                                PassType pt = PassType.ByVal;
                                operation = tokens.Dequeue();

                                if (operation == "ref")
                                {
                                    pt = PassType.Ref;
                                    operation = tokens.Dequeue();
                                }
                                else if (operation == "out")
                                {
                                    pt = PassType.Out;
                                    operation = tokens.Dequeue();
                                }
                                
                                var sb = new StringBuilder(operation);

                                while ((operation = tokens.Dequeue()) != ")" && operation != ",")
                                    sb.AppendFormat(" {0}", operation);

                                var variable = (Variable)FromString(sb.ToString());

                                if (variable.Name != "__void")
                                    parameters.Add(new Parameter(variable, pt));
                            }

                            return new Function(identifier, type, parameters);
                            #endregion
                        }
                        else
                        {
                            throw new ParserException("Неопознанный символ.");
                        }
                    }
                    else if (!isConst)
                    {
                        return new Variable(identifier, type, value);
                    }
                    else
                    {
                        throw new ParserException("Для константы ожидается начальное значение.");
                    }
                }
                catch (KeyNotFoundException e)
                {
                    throw new ParserException("Некорректный тип данных.", e);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new ParserException("Некорректный тип данных.", e);
                }
                catch (InvalidOperationException e)
                {
                    throw new ParserException("Неожиданный конец оператора.", e);
                }
                catch (InvalidCastException e)
                {
                    throw new ParserException("Неверный идентификатор типа.", e);
                }
                catch (OverflowException e)
                {
                    throw new ParserException("Число слишком большое.", e);
                }
                catch (FormatException e)
                {
                    throw new ParserException("Неверный формат.", e);
                }
            }

            private const string SplitRegex = @"\b";
        }

        public string Name { get; internal set; }
        public Type Typeid { get; internal set; }

        private static Dictionary<string, Type> mTypes = new Dictionary<string, Type>()
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
