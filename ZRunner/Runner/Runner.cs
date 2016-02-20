using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    class Runner
    {
        private Types types = new Types();
        public void Run(List<TypeWord> SyntaxList)
        {
            int FinalPos = SyntaxList.Count - 1;
            Stack<TypeWord> Out = new Stack<TypeWord>();
            Calculator calc = new Calculator();
            for (int Pos = 0; Pos <= FinalPos; Pos++)
            {  
                switch(SyntaxList[Pos].type)
                {
                    case Type.Identifier:
                    case Type.Number:
                    case Type.String:
                    case Type.Charactor:
                        {
                            Out.Push(SyntaxList[Pos]);
                            break;
                        }

                    case Type.Operator:
                        {
                            switch (SyntaxList[Pos].word)
                            {
                                case "(": //逆波兰表达式不应该有括号，除非是强制类型转换
                                    {
                                        if (SyntaxList[Pos+2].word != ")")
                                        {
                                        throw new Exception("运行时错误：强制类型转换括号未闭合");
                                        }
                                        if(SyntaxList[++Pos].type!=Type.KeyWord)
                                        {
                                        throw new Exception("运行时错误：强制类型转换的括号里必须是类型");
                                        }
                                        TypeWord t = new TypeWord(); //类型转换时创建临时变量并返回。
                                        t.type = Type.Identifier;
                                        t.word = "$" + SymbolTable.GetCurrentTableCount();
                                        TypeWord t1 = Out.Pop(); //栈顶的TypeWord
                                        int Address = -1;
                                        if (t1.type == Type.Identifier)
                                        {
                                            Address = VM.Alloc(types.ExplicitTypeCast(types.GetIdentifierType(t1), VM.GetValue(SymbolTable.GetAddress(t1.word)), types.ChineseToType(SyntaxList[Pos].word)));
                                        }else
                                        {
                                            Address = VM.Alloc(types.ExplicitTypeCast(types.GetIdentifierType(t1), t1.word, types.ChineseToType(SyntaxList[Pos].word)));
                                        }
                                        CreateVariable(t, types.ChineseToType(SyntaxList[Pos].word), false, Address);
                                        Out.Push(t);
                                        Pos++;
                                        break;
                                    }
                                case "=":
                                    {
                                        TypeWord t1, t2;
                                        t2 = Out.Pop();
                                        t1 = Out.Pop();
                                        if(t1.type!=Type.Identifier)
                                        {
                                            Exception e = new Exception("运行时错误：赋值运算等号左边必须是标识符");
                                            throw e;
                                        }
                                        if (t2.type != Type.Identifier)
                                        {
                                            IdentifierType T = new IdentifierType(); //非标识符t2的具体类型
                                            switch(t2.type)
                                            {
                                                case Type.Charactor:T = IdentifierType.Char;break;
                                                case Type.Number:T = IdentifierType.Double;break;
                                                case Type.String:T = IdentifierType.String;break;
                                            }
                                            TypeWord t =new TypeWord(Type.Identifier,"$"+SymbolTable.GetCurrentTableCount()); //赋值时自动进行类型转换建立临时变量
                                            CreateVariable(t, types.GetIdentifierType(t1), false, VM.Alloc(types.ExplicitTypeCast(T, t2.word, types.GetIdentifierType(t1))));
                                            VM.SetValue(SymbolTable.GetAddress(t1.word),VM.GetValue(SymbolTable.GetAddress(t.word)));
                                        }
                                        else
                                        {
                                            TypeWord t = new TypeWord(Type.Identifier, "$" + SymbolTable.GetCurrentTableCount()); //赋值时自动进行类型转换建立临时变量
                                          //  Console.WriteLine(types.GetIdentifierType(t1)+" "+types.GetIdentifierType(t2));
                                            CreateVariable(t, types.GetIdentifierType(t1), false, VM.Alloc(types.ExplicitTypeCast(types.GetIdentifierType(t2), VM.GetValue(SymbolTable.GetAddress(t2.word)), types.GetIdentifierType(t1))));
                                            VM.SetValue(SymbolTable.GetAddress(t1.word), VM.GetValue(SymbolTable.GetAddress(t.word)));
                                        }
                                        break;
                                    }
                                case "+":
                                    {
                                        TypeWord t1, t2;
                                        t2 = Out.Pop();
                                        t1 = Out.Pop();

                                        TypeWord aa = calc.Add(t1, t2);
                                        //Console.WriteLine("加法结果：" + VM.GetValue(SymbolTable.GetAddress(aa.word)));
                                        Out.Push(aa);
                                        break;
                                    }
                                case "-":
                                    {
                                        TypeWord t1, t2;
                                        t2 = Out.Pop();
                                        t1 = Out.Pop();
                                        Out.Push(calc.Minus(t1, t2));
                                        break;
                                    }
                                case "*":
                                    {
                                        TypeWord t1, t2;
                                        t2 = Out.Pop();
                                        t1 = Out.Pop();
                                        Out.Push(calc.Multiply(t1, t2));
                                        break;
                                    }
                                case "/":
                                    {
                                        TypeWord t1, t2;
                                        t2 = Out.Pop();
                                        t1 = Out.Pop();
                                        Out.Push(calc.Divide(t1, t2));
                                        break;
                                    }
                            }
                            break;
                        }
                    case Type.KeyWord:
                        {
                            switch(SyntaxList[Pos].word)
                            {
                                case "显示":
                                    {
                                        TypeWord t = Out.Pop();
                                        if (t.type == Type.Identifier)
                                        {
                                            Console.Write(VM.GetValue(SymbolTable.GetAddress(t.word)));
                                        }else
                                        {
                                            Console.Write(t.word);
                                        }
                                        break;
                                    }
                                case "输入":
                                    {
                                        TypeWord t = new TypeWord(Type.Identifier, "$" + SymbolTable.GetCurrentTableCount());
                                        string str = Console.ReadLine();
                                        CreateVariable(t, IdentifierType.String, false, VM.Alloc(str));
                                        Out.Push(t);
                                        break;

                                    }
                            }
                            break;
                        }
                }
            }
        }

        private void CreateVariable(TypeWord tw, IdentifierType type, bool IfStatic, int Address, object Default = null, object ExtMsg1 = null, object ExtMsg2 = null, Dictionary<string, object> ChildTable = null)
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
    }
}
