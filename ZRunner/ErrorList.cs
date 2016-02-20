using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    static class ErrorList
    {
        private static List<string> errorlist = new List<string>();

        public static void AddItem(string str)
        {
            errorlist.Add(str);
        }

        public static bool IsEmpty()
        {
            if (errorlist.Count == 0) return true;
            else return false;
        }

        public static void Show()
        {
            Console.WriteLine("编译出现错误：");
            foreach(var x in errorlist)
            {
                Console.WriteLine(x);
            }
            return;
        }
    }
}
