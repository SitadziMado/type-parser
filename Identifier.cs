﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            None,
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
                Queue<string> tokens = new Queue<string>(
                    from v
                    in Regex.Split(code.Trim(), SplitRegex)
                    where v.Trim().Length > 0
                    select v.Trim()
                );

                bool isConst = false;

                var first = tokens.Dequeue();

                if (first == "const")
                    isConst = true;
                else if (first == "class")
                    return new Class(tokens.Dequeue(), typeof(object));

                try
                {
                    Type type = Type.GetType(first, true, false);
                    string identifier = tokens.Dequeue();
                    object value = null;
                    var parameters = new List<Parameter>();

                    if (tokens.Count > 1)
                    {
                        string operation = tokens.Dequeue();

                        if (operation == "=")
                        {
                            value = Convert.ChangeType(tokens.Dequeue(), type);

                            if (isConst)
                                return new Constant(identifier, type, value);
                            else
                                return new Variable(identifier, type, value);
                        }
                        else if (operation == "(")
                        {
                            #region Функция
                            operation = tokens.Dequeue();

                            while (operation != ")")
                            {
                                PassType pt = PassType.None;
                                operation = tokens.Dequeue();

                                if (operation == "ref")
                                {
                                    pt = PassType.Ref;
                                }
                                else if (operation == "out")
                                {
                                    pt = PassType.Out;
                                }
                                else
                                {
                                    var sb = new StringBuilder(operation);

                                    while ((operation = tokens.Dequeue()) != ")" && operation != ",")
                                        sb.AppendFormat(" {0}", operation);

                                    parameters.Add(new Parameter(
                                            (Variable)FromString(sb.ToString()),
                                            pt
                                        )
                                    );
                                }
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
                catch (TypeLoadException e)
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
            }

            private const string SplitRegex = @"\b";
        }

        public string Name { get; internal set; }
        public Type Typeid { get; internal set; }

        private Dictionary<string, Type> mTypes = new Dictionary<string, Type>()
        {
            { "int", typeof(int) }
        };
    }
}
