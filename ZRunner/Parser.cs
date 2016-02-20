using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    class Parser
    {
        private int ComparePriority(string Op1,string Op2) //比较运算符优先级，返回前者比后者优先级大多少
        {
            Dictionary<string, int> OperatorList = new Dictionary<string, int>() { { ";", 0 }, { "=", 0 }, { "+", 3 }, { "-", 3 }, { "*", 4 }, { "/", 4 }, { "%", 4 }, { "^", 5 } };
            return OperatorList[Op1] - OperatorList[Op2];
        }

        private bool CreateVariable(TypeWord tw, IdentifierType type, bool IfStatic,object Default = null, object ExtMsg1 = null, object ExtMsg2 = null, Dictionary<string, object> ChildTable = null)
        {
            if(tw.type!=Type.Identifier)
            {
                ErrorList.AddItem("不正确的变量声明");
                return false;
            }
            STList t = new STList(type, IfStatic, VM.Alloc(null), Default, ExtMsg1, ExtMsg2, ChildTable);
            SymbolTable.AddItem(tw.word, t);
            return true;
        }
        private bool GetAllValuesInSentence(ref int StartPos,List<TypeWord> WordList,List<TypeWord> Out)
        {
            int FinalPos = StartPos;
            if (!ToSentenceEnd(ref FinalPos, WordList)) return false;
            FinalPos--;
            GetSyntaxList(Out, WordList, StartPos, FinalPos);
            StartPos = FinalPos; //游标移动到句尾分号的上一个位置
            return true;
        }

        private bool GetNextValue(ref int StartPos,List<TypeWord> WordList,List<TypeWord> Out) //遇到括号则将括号中的值计算出来。括号中有逗号的会计算出每个逗号分隔的语句的值，并添加到Out
        {
            if (WordList[StartPos].word == "(")
            {
                int FinalPos = -1;
                int LBrackets = 0; //左括号个数
                int Pos1 = StartPos; //临时游标
                while ((Pos1 = FindNextInSentence(Pos1, "(", WordList)) != -1)
                {
                    LBrackets++;
                    Pos1++;
                }
                Pos1 = StartPos;
                for (int i=0;i<LBrackets;i++)
                {
                    Pos1 = FindNextInSentence(Pos1, ")", WordList);
                    if(Pos1==-1)
                    {
                        ErrorList.AddItem("错误：括号未闭合");
                        return false;
                    }
                    else
                    {
                        Pos1++;
                    }
                }
                FinalPos = Pos1 - 2; //Pos1的位置是最后一个右括号的下一个位置，FinalPos应该是最后一个右括号的前一个位置，故减2
                StartPos++;
                GetSyntaxList(Out, WordList, StartPos, FinalPos);
                StartPos = FinalPos + 1;
                return true;
            }else if(WordList[StartPos].type==Type.Charactor || WordList[StartPos].type == Type.Number || WordList[StartPos].type == Type.String || WordList[StartPos].type == Type.Identifier)
            {
                int FinalPos = FindNextInSentence(StartPos + 1, ",", WordList);
                if(FinalPos == -1) FinalPos = FindNextInSentence(StartPos + 1, ")", WordList) - 1;
                if (FinalPos == -2) { FinalPos = StartPos; ToSentenceEnd(ref FinalPos, WordList); FinalPos--; }
                GetSyntaxList(Out, WordList, StartPos, FinalPos);
                return true;
            }
            else
            {
                ErrorList.AddItem("错误：应该输入数值，字符，字符串或操作符或表达式");
                return false;
            }
        }
        private bool ToSentenceEnd(ref int Pos,List<TypeWord> WordList)
        {
            int FinalPos = WordList.Count - 1;
            while (Pos <= FinalPos && WordList[Pos].word != ";") Pos++;
            if (Pos > FinalPos)
            {
                ErrorList.AddItem("句尾缺少操作符\";\"");
                return false;
            }
            else
            {
                return true;
            }
        }
        private int FindNextInSentence(int StartPos,string s,List<TypeWord> WordList)
        {
            int Pos = StartPos;
            while(WordList[Pos].word!=s)
            {
                if (WordList[Pos].word == ";") return -1;
                else Pos++;
            }
            return Pos;
        }
        public void GetSyntaxList(List<TypeWord> Out,List<TypeWord> WordList,int StartPos,int FinalPos) //Out为储存结果的列表,StartPos为从WordList的多少项开始分析,FinalPos为结束分析的项
        {
            Stack<TypeWord> In = new Stack<TypeWord>();
            for(int Pos = StartPos; Pos <= FinalPos; Pos++)
            {
                switch(WordList[Pos].type)
                {
                    case Type.Operator:
                        {
                            switch (WordList[Pos].word)
                            {
                                case ";":
                                    {
                                        while (In.Count > 0)
                                        {
                                            Out.Add(In.Pop());
                                        }
                                        Out.Add(WordList[Pos]);
                                        break;
                                    }
                                case "(":
                                case ")":
                                case "+":
                                case "-":
                                case "*":
                                case "/":
                                case "%":
                                case "^":
                                case "=":
                                    {
                                        if(WordList[Pos].word=="(")
                                        {
                                            In.Push(WordList[Pos]);
                                            break;
                                        }
                                        else if(WordList[Pos].word==")")
                                        {
                                            if(In.Count==0)
                                            {
                                                ErrorList.AddItem("括号不匹配");
                                                In.Clear();
                                                ToSentenceEnd(ref Pos,WordList);
                                                break;
                                            }
                                            else
                                            {
                                                TypeWord t = new TypeWord();
                                                while (In.Count>0 && (t=In.Pop()).word!="(")
                                                {
                                                    Out.Add(t);
                                                }
                                                break;
                                            }
                                        }
                                        else if (In.Count == 0 || In.Peek().word=="(" || ComparePriority(WordList[Pos].word, In.Peek().word) > 0)
                                        {
                                            In.Push(WordList[Pos]);
                                            break;
                                        }
                                        else
                                        {
                                            TypeWord t = new TypeWord();
                                            while (In.Count > 0 && (t=In.Pop()).word!="(" && ComparePriority(WordList[Pos].word, t.word) <= 0 && t.word!="(")
                                            {
                                                    Out.Add(t);
                                            }
                                            if (t.word == "(") In.Push(t);
                                            else if (ComparePriority(WordList[Pos].word, t.word) > 0) In.Push(t);
                                            In.Push(WordList[Pos]);
                                            break;
                                        }
                                    }
                            }
                            break;
                        }
                    case Type.KeyWord:
                        {
                            IdentifierType IT = new IdentifierType();
                            string word = WordList[Pos].word;
                                switch (word)
                            {
                                case "整数型": IT = IdentifierType.Int; goto DeclareVariable;
                                case "长整型": IT = IdentifierType.Long; goto DeclareVariable;
                                case "短整型": IT = IdentifierType.Short; goto DeclareVariable;
                                case "无符号整数型": IT = IdentifierType.UInt; goto DeclareVariable;
                                case "无符号长整型": IT = IdentifierType.ULong; goto DeclareVariable;
                                case "无符号短整型": IT = IdentifierType.UShort; goto DeclareVariable;
                                case "小数型": IT = IdentifierType.Float; goto DeclareVariable;
                                case "双精度小数型": IT = IdentifierType.Double; goto DeclareVariable;
                                case "字符型": IT = IdentifierType.Char; goto DeclareVariable;
                                case "字符串": IT = IdentifierType.String; goto DeclareVariable;

                                DeclareVariable:
                                    {
                                        if (CreateVariable(WordList[++Pos], IT, false) == false)
                                        { ToSentenceEnd(ref Pos, WordList); }
                                        else
                                        {
                                            Out.Add(WordList[Pos]); //将创建的变量名添加到输出列表中
                                          /*  if (WordList[Pos+1].word == "=")
                                            {
                                                Pos+=2;
                                                if (GetAllValuesInSentence(ref Pos, WordList, Out) == false)
                                                {
                                                    ToSentenceEnd(ref Pos, WordList);
                                                    break;
                                                }
                                                TypeWord t = new TypeWord();
                                                t.type = Type.Operator;
                                                t.word = "(";
                                                Out.Add(t);
                                                t.type = Type.KeyWord;
                                                t.word = word;
                                                Out.Add(t);
                                                t.type = Type.Operator;
                                                t.word = ")";
                                                Out.Add(t); //在声明变量赋初始值为数字时，由于数字默认按Double处理，故需自动添加强制类型转换
                                                Out.Add(new TypeWord(Type.Operator,"="));
                                                Out.Add(WordList[++Pos]); //将句尾的分号加入
                                            } */
                                        }
                                        break;
                                    }

                                case "显示":
                                    {
                                        Pos++;
                                        GetNextValue(ref Pos, WordList, Out);
                                        Out.Add(new TypeWord(Type.KeyWord,"显示")); //将“显示”关键字加入。
                                        break;
                                    }
                                case "输入":
                                    {
                                        if(WordList[++Pos].word != "(" || WordList[++Pos].word != ")")
                                        { ErrorList.AddItem("输入关键字后应该加括号"); ToSentenceEnd(ref Pos, WordList);break; }
                                        Out.Add(new TypeWord(Type.KeyWord, "输入")); //将“输入”关键字加入。
                                        break;
                                    }
                            }
                            break;
                        }
                    case Type.Identifier:
                        {
                            if (SymbolTable.IfExisted(WordList[Pos].word) == false)
                            {
                                ErrorList.AddItem("使用了未声明的变量" + WordList[Pos].word);
                                ToSentenceEnd(ref Pos, WordList);
                                break;
                            }
                            Out.Add(WordList[Pos]);
                            break;
                        }
                    case Type.String:
                        {
                            Out.Add(WordList[Pos]);
                            break;
                        }
                    case Type.Charactor: { Out.Add(WordList[Pos]);  break; }
                    case Type.Number: { Out.Add(WordList[Pos]);break; }
                }
            }
            while (In.Count > 0) //最后表达式结束后In栈需要解栈,因为有可能表达式不以";"结尾
            {
                Out.Add(In.Pop());
            }
        }
    }
}
