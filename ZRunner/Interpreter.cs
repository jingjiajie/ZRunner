using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{

    class Interpreter  //解释器类是解释工作的主控制类
    {
        private Lexer lexer = new Lexer();
        private Parser parser = new Parser();
        private Runner runner = new Runner();

        public void Run(string Source)
        {
            if(Defines.CurrentMode == Defines.Mode.Debug) Console.WriteLine("源程序：\n\n" + Source+"\n\n"); //调试模式输出源程序
            List<TypeWord> WordList = this.lexer.GetWordList(Source);
            if (WordList.Count == 0) return;
            if (Defines.CurrentMode == Defines.Mode.Debug) //调试模式输出WordList
            {
                foreach(var i in WordList)
                {
                    Console.Write("["+i.word+","+i.type+"]");
                }
                Console.WriteLine("\n\n");
            }
            List<TypeWord> SyntaxList = new List<TypeWord>();
            this.parser.GetSyntaxList(SyntaxList,WordList,0, WordList.Count - 1);
            if (Defines.CurrentMode == Defines.Mode.Debug) //调试模式输出逆波兰表达式
            {
                Console.WriteLine("翻译为逆波兰表达式代码：\n");
                foreach (var i in SyntaxList)
                {
                    Console.Write(i.word+" ");
                }
                Console.WriteLine("\n");
            }

            if(!ErrorList.IsEmpty())
            {
                ErrorList.Show();
                return;
            } //如果有错误则显示错误列表，然后停止运行。

            try
            {
                this.runner.Run(SyntaxList);
            }catch(Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
