using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    static class VM //虚拟内存类
    {
        private static List<object> VisualMemory = new List<object>(1024); //虚拟内存

        public static int Alloc(object Value) //Content为要存储的内容。返回储存的内存地址
        {
            VisualMemory.Add(Value);
            return VisualMemory.Count - 1;
        }

        public static object GetValue(int Address)
        {
            return VisualMemory[Address];
        }
        public static void SetValue(int Address,object Value)
        {
            VisualMemory[Address] = Value;
        }
    }
}
