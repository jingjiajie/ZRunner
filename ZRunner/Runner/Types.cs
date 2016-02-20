using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    class Types
    {
        public object ExplicitTypeCast(IdentifierType TSource, object SourceValue, IdentifierType TResult)
        {
            
            object ResultValue = null;
            switch (TResult)
            {
                case IdentifierType.Char:
                    {

                        if (TSource == IdentifierType.String)
                        {
                            if (SourceValue.ToString().Length == 1) { ResultValue = Convert.ToChar(SourceValue); break; }
                            else throw new Exception("运行时错误：字符串长度必须为1才能强制转换为字符型");
                        }
                        else if (TSource == IdentifierType.Int || TSource == IdentifierType.Short || TSource == IdentifierType.Long || TSource==IdentifierType.Float || TSource==IdentifierType.Double) { ResultValue = Convert.ToChar(Convert.ToInt64(SourceValue)); break; }
                        else if (TSource == IdentifierType.Char) { ResultValue = SourceValue; break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");

                    }
                case IdentifierType.Double:
                    {
                        //Console.WriteLine(TSource.ToString() + " " + TResult.ToString());
                        if (TSource == IdentifierType.String) { throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型"); }
                        else if (TSource == IdentifierType.Char) { ResultValue = Convert.ToDouble(Convert.ToInt64(SourceValue)); break; }
                        else { ResultValue = Convert.ToDouble(SourceValue); break; }
                    }
                case IdentifierType.Float:
                    {
                        if (TSource != IdentifierType.String) { ResultValue = Convert.ToSingle(SourceValue); break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");
                    }
                case IdentifierType.Int:
                    {
                        if (TSource != IdentifierType.String) { ResultValue = Convert.ToInt32(SourceValue); break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");
                    }
                case IdentifierType.Long:
                    {
                        if (TSource != IdentifierType.String) { ResultValue = Convert.ToInt64(SourceValue); break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");
                    }
                case IdentifierType.Short:
                    {
                        if (TSource != IdentifierType.String) { ResultValue = Convert.ToInt16(SourceValue); break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");
                    }
                case IdentifierType.UInt:
                    {
                        if (TSource != IdentifierType.String) { ResultValue = Convert.ToUInt32(SourceValue); break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");
                    }
                case IdentifierType.ULong:
                    {
                        if (TSource != IdentifierType.String) { ResultValue = Convert.ToUInt64(SourceValue); break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");
                    }
                case IdentifierType.UShort:
                    {
                        if (TSource != IdentifierType.String) { ResultValue = Convert.ToUInt16(SourceValue); break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型");
                    }
                case IdentifierType.String: ResultValue = Convert.ToString(SourceValue); break;
                default: Exception e = new Exception("运行时错误：无法将" + TSource + " 强制转换到" + TResult + "类型"); throw e;
            }
            return ResultValue;
        }
        public object TypeCast(IdentifierType TSource, object SourceValue, IdentifierType TResult) //类型转换函数
        {
            object ResultValue = null;
           
            if (GetPriorIdentifierType(TSource, TResult) != TResult)
            {
                Exception e = new Exception("运行时错误：无法将" + TSource + " 转换到" + TResult + "类型");
                throw e;
            }
            switch (TResult)
            {
                case IdentifierType.Char:
                    {
                        if (TSource == IdentifierType.String)
                        {
                            if (SourceValue.ToString().Length == 1) { ResultValue = Convert.ToChar(SourceValue); break; }
                            else throw new Exception("运行时错误：字符串长度必须为1才能转换为字符型");
                        }
                        else if (TSource == IdentifierType.Int || TSource == IdentifierType.Short || TSource == IdentifierType.Long) { ResultValue = Convert.ToChar(Convert.ToInt64(SourceValue)); break; }
                        else if (TSource == IdentifierType.Char) { ResultValue = SourceValue; break; }
                        else throw new Exception("运行时错误：无法将" + TSource + " 转换到" + TResult + "类型");
                    }
                case IdentifierType.Double:
                    {
                        if (TSource == IdentifierType.Float) { ResultValue = Convert.ToDouble(SourceValue.ToString()); break; }
                        else { ResultValue = Convert.ToDouble(SourceValue); break; }
                    }
                case IdentifierType.Float: ResultValue = Convert.ToSingle(SourceValue); break;
                case IdentifierType.Int: ResultValue = Convert.ToInt32(SourceValue); break;
                case IdentifierType.Long: ResultValue = Convert.ToInt64(SourceValue); break;
                case IdentifierType.Short: ResultValue = Convert.ToInt16(SourceValue); break;
                case IdentifierType.String: ResultValue = Convert.ToString(SourceValue); break;
                case IdentifierType.UInt: ResultValue = Convert.ToUInt32(SourceValue); break;
                case IdentifierType.ULong: ResultValue = Convert.ToUInt64(SourceValue); break;
                case IdentifierType.UShort: ResultValue = Convert.ToUInt16(SourceValue); break;
                default: Exception e = new Exception("运行时错误：无法将" + TSource + " 转换到" + TResult + "类型"); throw e;
            }

            return ResultValue;
        }

        public IdentifierType GetIdentifierType(TypeWord t)
        {
            IdentifierType r = new IdentifierType();
            switch(t.type)
            {
                case Type.Number: r = IdentifierType.Double;break;
                case Type.Charactor:r = IdentifierType.Char; break;
                case Type.String:r = IdentifierType.String; break;
                case Type.Identifier: r=SymbolTable.GetTypeByName(t.word); break;
                case Type.KeyWord:
                    {
                        r = ChineseToType(t.word);
                        break;
                    }
                default:throw new Exception("运行时错误：无法获取"+t.word+"的数据类型");
            }
            return r;
        }
        public IdentifierType ChineseToType(string t)
        {
            IdentifierType r = new IdentifierType();
            switch (t)
            {
                case "字符型": r = IdentifierType.Char ; break;
                case "双精度小数型": r = IdentifierType.Double; break;
                case "小数型": r = IdentifierType.Float; break;
                case "整数型": r = IdentifierType.Int; break;
                case "长整型": r = IdentifierType.Long; break;
                case "短整型": r = IdentifierType.Short; break;
                case "无符号整数型": r = IdentifierType.UInt; break;
                case "无符号长整型": r = IdentifierType.ULong; break;
                case "无符号短整型": r = IdentifierType.Short; break;
                case "字符串": r = IdentifierType.String; break;
                default:throw new Exception("无法将" + t + "转换为类型");
            }
            return r;
        }
        public string TypeToChinese(IdentifierType t)
        {
            string r = null;
            switch (t)
            {
                case IdentifierType.Char: r = "字符型"; break;
                case IdentifierType.Double: r = "双精度小数型"; break;
                case IdentifierType.Float: r = "小数型"; break;
                case IdentifierType.Int: r = "整数型"; break;
                case IdentifierType.Long: r = "长整型"; break;
                case IdentifierType.Short: r = "短整型"; break;
                case IdentifierType.UInt: r = "无符号整数型"; break;
                case IdentifierType.ULong: r = "无符号长整型"; break;
                case IdentifierType.UShort: r = "无符号短整型"; break;
                case IdentifierType.String: r = "字符串"; break;
                default: r = null; break;
            }
            return r;
        }
        public IdentifierType GetPriorIdentifierType(TypeWord t1, TypeWord t2)
        {
            IdentifierType t = new IdentifierType();
            if (t1.type == Type.String || t2.type == Type.String)
            {
                t = IdentifierType.String;
            }
            else if (t1.type == Type.Number && t2.type == Type.Number)
            {
                t = IdentifierType.Double;
            }
            else if (t1.type == Type.Number && t2.type == Type.Charactor)
            {
                t = IdentifierType.Double;
            }
            else if (t1.type == Type.Number && t2.type == Type.Identifier)
            {
                t = GetPriorIdentifierType(IdentifierType.Double, SymbolTable.GetTypeByName(t2.word));
            }
            else if (t1.type == Type.Charactor && t2.type == Type.Charactor)
            {
                t = IdentifierType.Char;
            }
            else if (t1.type == Type.Charactor && t2.type == Type.Number)
            {
                t = IdentifierType.Double;
            }
            else if (t1.type == Type.Charactor && t2.type == Type.Identifier)
            {
                t = GetPriorIdentifierType(IdentifierType.Char, SymbolTable.GetTypeByName(t2.word));
            }
            else if (t1.type == Type.Identifier && t2.type == Type.Number)
            {
                t = GetPriorIdentifierType(IdentifierType.Double, SymbolTable.GetTypeByName(t1.word));
            }
            else if (t1.type == Type.Identifier && t2.type == Type.Charactor)
            {
                t = GetPriorIdentifierType(IdentifierType.Char, SymbolTable.GetTypeByName(t1.word));
            }
            else if (t1.type == Type.Identifier && t2.type == Type.Identifier)
            {
                t = GetPriorIdentifierType(SymbolTable.GetTypeByName(t1.word), SymbolTable.GetTypeByName(t2.word));
            }
            return t;
        }

        public IdentifierType GetPriorIdentifierType(IdentifierType t1, IdentifierType t2)
        {
            int p1 = GetIdentifierTypePriority(t1);
            int p2 = GetIdentifierTypePriority(t2);
            if (p1 < 0 || p2 < 0)
            {
                Exception e = new Exception("运行时错误：无法比较" + t1.ToString() + "与" + t2.ToString() + "的优先级");
                throw e;
            }
            else if (p1 > p2)
            {
                return t1;
            }
            else
            {
                return t2;
            }
        }

        private int GetIdentifierTypePriority(IdentifierType t)
        {
            //类型优先值: char<short<ushort<int<uint<long<ulong<float<double<string
            int p = 0; //运算符优先值

            switch (t)
            {
                case IdentifierType.Char: p = 0; break;
                case IdentifierType.Short: p = 1; break;
                case IdentifierType.UShort: p = 2; break;
                case IdentifierType.Int: p = 3; break;
                case IdentifierType.UInt: p = 4; break;
                case IdentifierType.Long: p = 5; break;
                case IdentifierType.ULong: p = 6; break;
                case IdentifierType.Float: p = 7; break;
                case IdentifierType.Double: p = 8; break;
                case IdentifierType.String: p = 9; break;
                default: p = -1; break;
            }

            return p;
        }
    }
}
