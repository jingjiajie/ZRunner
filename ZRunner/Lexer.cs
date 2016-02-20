using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    public enum Type { Operator, KeyWord, Identifier, String, Charactor, Number, Comment, Precompile, Unknown,Null };
    public enum IdentifierType { Class,Function,Custom,Typedef, Int,Long,Short,UInt,ULong,UShort,Float,Double,Char,String,Unknown,Null };
    public struct TypeWord
    {
        public Type type;
        public string word;

        public TypeWord(Type type,string word)
        {
            this.type = type;
            this.word = word;
            return;
        }
    }
    class Lexer
    {

        private Dictionary<string, int> KeyWords; //所有关键字
        private Dictionary<string, int> Operators; //所有运算符
        StringBuilder t = new StringBuilder();
        public Lexer()
        {
            this.KeyWords = new Dictionary<string, int>() { { "显示", 1 }, { "输入",1},{ "抽象", 1 }, { "as", 1 }, { "基类", 1 }, { "布尔型", 1 }, { "跳出", 1 }, { "字节型", 1 }, { "情况", 1 }, { "捕获", 1 }, { "字符型", 1 }, { "checked", 1 }, { "类", 1 }, { "常量", 1 }, { "继续", 1 }, { "decimal", 1 }, { "默认", 1 }, { "委托", 1 }, { "判断循环首", 1 }, { "双精度小数型", 1 }, { "否则", 1 }, { "枚举", 1 }, { "ecent", 1 }, { "explicit", 1 }, { "extern", 1 }, { "假", 1 }, { "finally", 1 }, { "fixed", 1 }, { "小数型", 1 }, { "计次循环", 1 }, { "foreach", 1 }, { "get", 1 }, { "跳转到", 1 }, { "如果", 1 }, { "implicit", 1 }, { "in", 1 }, { "整数型", 1 }, { "接口", 1 }, { "内部", 1 }, { "is", 1 }, { "lock", 1 }, { "长整数型", 1 }, { "命名空间", 1 }, { "新建", 1 }, { "空", 1 }, { "object", 1 }, { "out", 1 }, { "重写", 1 }, { "partial", 1 }, { "私有", 1 }, { "保护", 1 }, { "公共", 1 }, { "只读", 1 }, { "引用", 1 }, { "返回", 1 }, { "sbyte", 1 }, { "sealed", 1 }, { "set", 1 }, { "短整数型", 1 }, { "大小", 1 }, { "stackalloc", 1 }, { "静态", 1 }, { "字符串", 1 }, { "结构体", 1 }, { "开关", 1 }, { "本类", 1 }, { "抛出", 1 }, { "真", 1 }, { "尝试", 1 }, { "typeof", 1 }, { "无符号整数型", 1 }, { "无符号长整型", 1 }, { "unchecked", 1 }, { "unsafe", 1 }, { "无符号短整型", 1 }, { "使用", 1 }, { "value", 1 }, { "虚函数", 1 }, { "虚类", 1 }, { "volatile", 1 }, { "空类型", 1 }, { "where", 1 }, { "判断循环", 1 }, { "yield", 1 } };
            this.Operators = new Dictionary<string, int>() { { "[", 1 }, { "]", 1 }, { "(", 1 }, { ")", 1 }, { "{", 1 }, { "}", 1 }, { "++", 1 }, { "--", 1 }, { "~", 1 }, { "!", 1 }, { "+", 1 }, { "-", 1 }, { "&", 1 }, { "*", 1 }, { "/", 1 }, { "%", 1 }, { ">>", 1 }, { "<<", 1 }, { ">", 1 }, { "<", 1 }, { ">=", 1 }, { "<=", 1 }, { "==", 1 }, { "!=", 1 }, { "|", 1 }, { "^", 1 }, { "&&", 1 }, { "||", 1 }, { "?", 1 }, { ":", 1 }, { "=", 1 }, { "+=", 1 }, { "-=", 1 }, { "*=", 1 }, { "/=", 1 }, { "%=", 1 }, { "|=", 1 }, { "&=", 1 }, { "^=", 1 }, { "<<=", 1 }, { ">>=", 1 }, { ",", 1 }, { ".", 1 }, { ";", 1 } };
        }

        public bool IsCharactor(string str)
        {
            if (str.Length != 1 && str.StartsWith("\'") && str.EndsWith("\'")) return true;
            else return false;
        }
        public bool IsNumber(string str)
        {
            bool IfContainsPoint = false; //是否包含小数点
            if (str[0] < '0' || str[0] > '9') return false; //第一个字母不是数字的一定不是数字
            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == '.')
                {
                    if (IfContainsPoint == false) IfContainsPoint = true;
                    else return false;
                }
            }
            return true;
        }
        public bool IsKeyWord(string str)
        {
            if (this.KeyWords.ContainsKey(str))
                return true;
            else
                return false;
        }

        public bool IsOperator(string str)
        {
            if (this.Operators.ContainsKey(str))
                return true;
            else
                return false;
        }

        public bool IsString(string str)
        {
            if (str.Length > 1 && str.StartsWith("\"") && str.EndsWith("\"")) return true;
            else return false;
        }

        public bool IsComment(string str)
        {
            if (str.StartsWith("//")) return true;
            else return false;
        }

        public TypeWord Next(ref int StartPos, string Str) //提取某行的下一个单词或操作符，提取后StartPos滑动到单词或操作符的下一个位置
        {
            this.t.Length = 0;
            int FinalPos = Str.Length - 1;
            TypeWord typeword = new TypeWord();
            typeword.type = Type.Null;
            if (StartPos > FinalPos)
            {
                typeword.type = Type.Null;
                typeword.word = null;
                return typeword;
            }

            while (true)
            {
                while (StartPos <= FinalPos
                    && Str[StartPos] != ' '
                    && Str[StartPos] != '\t'
                    && Str[StartPos] != '\n'
                    && Str[StartPos] != '\r'
                    )
                {
                    if (t.Length == 0 && Str[StartPos] == '\"') //需要单独处理字符串，因为可能包含空格，注释符等特殊符号
                    {
                        t.Append(Str[StartPos]);
                        if (++StartPos > FinalPos) break; //处理最后一个字符为"的特殊情况
                        int StrPos1 = StartPos;
                        while (StrPos1 <= FinalPos && ((Str[StrPos1] == '\"') ? (Str[StrPos1 - 1] == '\\' ? true : false) : true))
                        {
                            StrPos1++;
                        }
                        if (StrPos1 <= FinalPos) //是字符串的情况
                        {
                            for (; StartPos <= StrPos1; StartPos++)
                            {
                                t.Append(Str[StartPos]);
                            }
                            typeword.type = Type.String;
                            typeword.word = t.ToString().Substring(1,t.Length-2);
                            break;
                        }
                        else { continue; } //此种情况不是字符串
                    }

                    if (t.Length == 0 && Str[StartPos] == '\'') //单独处理字符型
                    {
                        if (++StartPos > FinalPos) break; //处理最后一个字符为'的特殊情况
                        int StrPos1 = StartPos;
                        while (StrPos1 <= StartPos + 3 && ((Str[StrPos1] == '\'') ? (Str[StrPos1 - 1] == '\\' ? true : false) : true))
                        {
                            StrPos1++;
                        }
                        if ((StrPos1==StartPos+1)||(StrPos1 == StartPos + 2 && Str[StartPos] == '\\')) //是字符的情况
                        {
                            for (; StartPos < StrPos1; StartPos++)
                            {
                                t.Append(Str[StartPos]);
                                Console.WriteLine("t.append:" + Str[StartPos]);
                            }
                            StartPos++;
                            typeword.type = Type.Charactor;
                            typeword.word = t.ToString();
                            break;
                        }
                        else { continue; } //此种情况不是字符
                    }

                    if (t.Length == 0 && StartPos < FinalPos && Str[StartPos] == '/' && Str[StartPos + 1] == '/')
                    {
                        while (StartPos <= FinalPos)
                        {
                            t.Append(Str[StartPos]);
                            StartPos++;
                        }
                        typeword.type = Type.Comment;
                        typeword.word = t.ToString();
                        break;
                    }

                    if (t.Length == 0 && this.IsOperator(Str[StartPos].ToString()))
                    {
                        t.Append(Str[StartPos]);
                        typeword.type = Type.Operator;
                        while (true)
                        {
                            if (StartPos < FinalPos) t.Append(Str[++StartPos]);
                            else { StartPos++; typeword.word = t.ToString(); return typeword; }
                            if (this.IsOperator(t.ToString())) continue;
                            else { typeword.word = t.Remove(t.Length - 1, 1).ToString(); return typeword; }
                        }
                    }

                    if (this.IsOperator(Str[StartPos].ToString()) || Str[StartPos] == '\"')
                    {
                        if (!(Str[StartPos] == '.' && IsNumber(t.ToString()))) //处理小数的情况
                        {
                            break;
                        }
                    }

                    t.Append(Str[StartPos]);
                    StartPos++;
                }
                if (t.Length != 0) //如果取到词了就返回。如果什么都没有取到（第一个字符为运算符）就再往后取词
                {
                    if (typeword.word == null)
                    {
                        typeword.word = t.ToString();
                        if (this.IsKeyWord(typeword.word)) typeword.type = Type.KeyWord;
                        else if (this.IsCharactor(typeword.word)) typeword.type = Type.Charactor;
                        else if (this.IsNumber(typeword.word)) typeword.type = Type.Number;
                        else { typeword.type = Type.Identifier; }
                    }
                    break;
                }
                else if (StartPos <= FinalPos)
                {
                    StartPos++;
                    continue;
                }
                else if (StartPos > FinalPos)
                {
                    break;
                }
            }

            return typeword;
        }

        public List<TypeWord> GetWordList(string Source)
        {
            int StartPos = 0;
            List<TypeWord> WordList = new List<TypeWord>();
            TypeWord t;
            while ((t = this.Next(ref StartPos, Source)).word != null)
            {
                WordList.Add(t);
            }
            return WordList;
        }
    }
}
