using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    class Calculator
    {
        private Types types = new Types();
        public TypeWord Add(TypeWord tw1,TypeWord tw2)
        {
            IdentifierType t = types.GetPriorIdentifierType(tw1, tw2); //隐式转换的类型
            object t1 = GetValue(tw1);
            object t2 = GetValue(tw2);
            object sum = null;
            TypeWord r = new TypeWord();
            r.type = Type.Identifier; //加法运算创建临时变量并返回。
            r.word = "$" + SymbolTable.GetCurrentTableCount();
             switch (t)
             {
                 case IdentifierType.Char: { sum=Convert.ToChar(t1) + Convert.ToChar(t2); break; }
                 case IdentifierType.Short: { sum = Convert.ToInt16(t1) + Convert.ToInt16(t2); break; }
                 case IdentifierType.UShort: { sum = Convert.ToUInt16(t1) + Convert.ToUInt16(t2); break; }
                 case IdentifierType.Int: { sum = Convert.ToInt32(t1) + Convert.ToInt32(t2); break; }
                 case IdentifierType.UInt: { sum = Convert.ToUInt32(t1) + Convert.ToUInt32(t2); break; }
                 case IdentifierType.Long: { sum = Convert.ToInt64(t1) + Convert.ToInt64(t2); break; }
                 case IdentifierType.ULong: { sum = Convert.ToUInt64(t1) + Convert.ToUInt64(t2); break; }
                 case IdentifierType.Float: { sum = Convert.ToSingle(t1) + Convert.ToSingle(t2); break; }
                 case IdentifierType.Double:
                    {
                        if (types.GetIdentifierType(tw1) == IdentifierType.Float || types.GetIdentifierType(tw2) == IdentifierType.Float)
                        { sum = Convert.ToDouble(t1.ToString()) + Convert.ToDouble(t2.ToString()); } //避免Float转换Double精度丢失的缺陷
                        else if (types.GetIdentifierType(tw1) == IdentifierType.Char || types.GetIdentifierType(tw2) == IdentifierType.Char)
                        { sum = Convert.ToDouble(Convert.ToInt32(t1)) + Convert.ToDouble(Convert.ToInt32(t2)); }
                        else { sum = Convert.ToDouble(t1) + Convert.ToDouble(t2); }
                        break; 
                    }
                 case IdentifierType.String: { sum = Convert.ToString(t1) + Convert.ToString(t2); break; }
                 default:
                     {
                         Exception e = new Exception("运行时错误：无法处理" + t1.ToString() + "与" + t2.ToString() + "的相加运算");
                         throw e;
                     }
             }
            CreateVariable(r, t, false, VM.Alloc(sum));
            return r;
        }

        public TypeWord Minus(TypeWord tw1, TypeWord tw2)
        {
            IdentifierType t = types.GetPriorIdentifierType(tw1, tw2); //隐式转换的类型
            object t1 = GetValue(tw1);
            object t2 = GetValue(tw2);
            object sum = null;
            TypeWord r = new TypeWord();
            r.type = Type.Identifier; //加法运算创建临时变量并返回。
            r.word = "$" + SymbolTable.GetCurrentTableCount();
            switch (t)
            {
                case IdentifierType.Char: { sum = Convert.ToChar(t1) - Convert.ToChar(t2); break; }
                case IdentifierType.Short: { sum = Convert.ToInt16(t1) - Convert.ToInt16(t2); break; }
                case IdentifierType.UShort: { sum = Convert.ToUInt16(t1) - Convert.ToUInt16(t2); break; }
                case IdentifierType.Int: { sum = Convert.ToInt32(t1) - Convert.ToInt32(t2); break; }
                case IdentifierType.UInt: { sum = Convert.ToUInt32(t1) - Convert.ToUInt32(t2); break; }
                case IdentifierType.Long: { sum = Convert.ToInt64(t1) - Convert.ToInt64(t2); break; }
                case IdentifierType.ULong: { sum = Convert.ToUInt64(t1) - Convert.ToUInt64(t2); break; }
                case IdentifierType.Float: { sum = Convert.ToSingle(t1) - Convert.ToSingle(t2); break; }
                case IdentifierType.Double:
                    {
                        if (types.GetIdentifierType(tw1) == IdentifierType.Float || types.GetIdentifierType(tw2) == IdentifierType.Float)
                        { sum = Convert.ToDouble(t1.ToString()) - Convert.ToDouble(t2.ToString()); } //避免Float转换Double精度丢失的缺陷
                        else if (types.GetIdentifierType(tw1) == IdentifierType.Char || types.GetIdentifierType(tw2) == IdentifierType.Char)
                        { sum = Convert.ToDouble(Convert.ToInt32(t1)) - Convert.ToDouble(Convert.ToInt32(t2)); }
                        else { sum = Convert.ToDouble(t1) - Convert.ToDouble(t2); }
                        break;
                    }
                default:
                    {
                        Exception e = new Exception("运行时错误：无法处理" + t1.ToString() + "与" + t2.ToString() + "的相减运算");
                        throw e;
                    }
            }
            CreateVariable(r, t, false, VM.Alloc(sum));
            return r;
        }

        public TypeWord Multiply(TypeWord tw1, TypeWord tw2)
        {
            IdentifierType t = types.GetPriorIdentifierType(tw1, tw2); //隐式转换的类型
            object t1 = GetValue(tw1);
            object t2 = GetValue(tw2);
            object sum = null;
            TypeWord r = new TypeWord();
            r.type = Type.Identifier; //加法运算创建临时变量并返回。
            r.word = "$" + SymbolTable.GetCurrentTableCount();
            switch (t)
            {
                case IdentifierType.Char: { sum = Convert.ToChar(t1) * Convert.ToChar(t2); break; }
                case IdentifierType.Short: { sum = Convert.ToInt16(t1) * Convert.ToInt16(t2); break; }
                case IdentifierType.UShort: { sum = Convert.ToUInt16(t1) * Convert.ToUInt16(t2); break; }
                case IdentifierType.Int: { sum = Convert.ToInt32(t1) * Convert.ToInt32(t2); break; }
                case IdentifierType.UInt: { sum = Convert.ToUInt32(t1) * Convert.ToUInt32(t2); break; }
                case IdentifierType.Long: { sum = Convert.ToInt64(t1) * Convert.ToInt64(t2); break; }
                case IdentifierType.ULong: { sum = Convert.ToUInt64(t1) * Convert.ToUInt64(t2); break; }
                case IdentifierType.Float: { sum = Convert.ToSingle(t1) * Convert.ToSingle(t2); break; }
                case IdentifierType.Double:
                    {
                        if (types.GetIdentifierType(tw1) == IdentifierType.Float || types.GetIdentifierType(tw2) == IdentifierType.Float)
                        { sum = Convert.ToDouble(t1.ToString()) * Convert.ToDouble(t2.ToString()); } //避免Float转换Double精度丢失的缺陷
                        else if (types.GetIdentifierType(tw1) == IdentifierType.Char || types.GetIdentifierType(tw2) == IdentifierType.Char)
                        { sum = Convert.ToDouble(Convert.ToInt32(t1)) * Convert.ToDouble(Convert.ToInt32(t2)); }
                        else { sum = Convert.ToDouble(t1) * Convert.ToDouble(t2); }
                        break;
                    }
                default:
                    {
                        Exception e = new Exception("运行时错误：无法处理" + t1.ToString() + "与" + t2.ToString() + "的相乘运算");
                        throw e;
                    }
            }
            CreateVariable(r, t, false, VM.Alloc(sum));
            return r;
        }

        public TypeWord Divide(TypeWord tw1, TypeWord tw2)
        {
            IdentifierType t = types.GetPriorIdentifierType(tw1, tw2); //隐式转换的类型
            object t1 = GetValue(tw1);
            object t2 = GetValue(tw2);
            object sum = null;
            TypeWord r = new TypeWord();
            r.type = Type.Identifier; //加法运算创建临时变量并返回。
            r.word = "$" + SymbolTable.GetCurrentTableCount();
            switch (t)
            {
                case IdentifierType.Char: { sum = Convert.ToChar(t1) / Convert.ToChar(t2); break; }
                case IdentifierType.Short: { sum = Convert.ToInt16(t1) / Convert.ToInt16(t2); break; }
                case IdentifierType.UShort: { sum = Convert.ToUInt16(t1) / Convert.ToUInt16(t2); break; }
                case IdentifierType.Int: { sum = Convert.ToInt32(t1) / Convert.ToInt32(t2); break; }
                case IdentifierType.UInt: { sum = Convert.ToUInt32(t1) / Convert.ToUInt32(t2); break; }
                case IdentifierType.Long: { sum = Convert.ToInt64(t1) / Convert.ToInt64(t2); break; }
                case IdentifierType.ULong: { sum = Convert.ToUInt64(t1) / Convert.ToUInt64(t2); break; }
                case IdentifierType.Float: { sum = Convert.ToSingle(t1) / Convert.ToSingle(t2); break; }
                case IdentifierType.Double:
                    {
                        if (types.GetIdentifierType(tw1) == IdentifierType.Float || types.GetIdentifierType(tw2) == IdentifierType.Float)
                        { sum = Convert.ToDouble(t1.ToString()) / Convert.ToDouble(t2.ToString()); } //避免Float转换Double精度丢失的缺陷
                        else if (types.GetIdentifierType(tw1) == IdentifierType.Char || types.GetIdentifierType(tw2) == IdentifierType.Char)
                        { sum = Convert.ToDouble(Convert.ToInt32(t1)) / Convert.ToDouble(Convert.ToInt32(t2)); }
                        else { sum = Convert.ToDouble(t1) / Convert.ToDouble(t2); }
                        break;
                    }
                default:
                    {
                        Exception e = new Exception("运行时错误：无法处理" + t1.ToString() + "与" + t2.ToString() + "的相除运算");
                        throw e;
                    }
            }
            CreateVariable(r, t, false, VM.Alloc(sum));
            return r;
        }

        private void CreateVariable(TypeWord tw, IdentifierType type, bool IfStatic,int Address, object Default = null, object ExtMsg1 = null, object ExtMsg2 = null, Dictionary<string, object> ChildTable = null)
        {
            if (tw.type != Type.Identifier)
            {
                ErrorList.AddItem("不正确的变量声明");
                return;
            }
            STList t = new STList(type, IfStatic, Address, Default, ExtMsg1, ExtMsg2, ChildTable);
            SymbolTable.AddItem(tw.word, t);
            return;

        }

        private object GetValue(TypeWord t)
        {
            if (t.type != Type.Identifier) return t.word;
            else { return VM.GetValue(SymbolTable.GetAddress(t.word)); }
        }
    }
}
