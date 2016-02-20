using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    class ZRunner
    {
        static void Main(string[] args)
        {
            string FilePath = null;
            string Source = null;
            if (args.Length == 1)
            {
                FilePath = args[0];
            }
            else
            {
                Console.WriteLine("未指定要运行的文件\n用法：\nZRunner.exe [要运行的源代码文件]\n\n按任意键结束...");
                Console.ReadLine();
                return;
            }

            Files F = new Files();
            try
            {
                byte[] FileContent = F.ReadFile(FilePath);
                Source = System.Text.Encoding.Default.GetString(FileContent);
            }
            catch (Exception e) { Console.WriteLine(e.Message); return; }  
           // string Source = "整数型 变量1 = 1;小数型 变量2 = 变量1+1;显示(变量1+1.1);";
            Interpreter interpreter = new Interpreter();
            if (ErrorList.IsEmpty()) interpreter.Run(Source);
            else ErrorList.Show();
            Console.ReadLine();
        }
    }
}
