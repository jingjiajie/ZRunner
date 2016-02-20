using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRunner
{
    static class SymbolTable
    {
        public static Dictionary<string, object> CurrentSymbolTable = new Dictionary<string, object>() { { "~ParentTable", null } };
        public static Dictionary<string, object> CreateSymbolTable(Dictionary<string, object> ParentTable)
        {
            Dictionary<string, object> t = new Dictionary<string, object>();
            t.Add("~ParentTable",ParentTable);
            return t;
        }
        public static void AddItem(string Name,STList list)
        {
            if(IfExisted(Name))
            {
                throw new Exception("声明了重复变量或函数");
            }
            CurrentSymbolTable.Add(Name, list);
        }

        public static bool IfExisted(string Name)
        {
            if (CurrentSymbolTable.ContainsKey(Name)) return true;
            else return false;
        }

        public static int GetAddress(string Name)
        {
            Dictionary<string, object> t = CurrentSymbolTable;
            while(t != null && !t.ContainsKey(Name))
            {
                t = GetParentTable(t);
            }
            if(t==null)
            {
                Exception e = new Exception("运行时错误：试图使用未声明的变量"+Name);
                throw e;
            }
            else if ((int)((STList)t[Name])[2] == -1)
            {
                Exception e = new Exception("运行时错误：试图使用实例化的变量"+Name);
                throw e;
            }
            else { return (int)((STList)t[Name])[2]; }                                                                                                                                     
        }

        public static void SetAddress(string Name, int Address)
        {
            Dictionary<string, object> t = CurrentSymbolTable;
            while (t != null && !t.ContainsKey(Name))
            {
                t = GetParentTable(t);
            }
            if (t == null)
            {
                Exception e = new Exception("运行时错误：试图使用未声明的变量");
                throw e;
            }
            else
            {
                ((STList)t[Name])[2] = Address;
            }
        }
        public static IdentifierType GetTypeByName(string Name)
        {
            Dictionary<string, object> t = CurrentSymbolTable;
            while (t != null && !t.ContainsKey(Name))
            {
                t = GetParentTable(t);
            }
            if (t == null)
            {
                Exception e = new Exception("运行时错误：试图使用未声明的变量");
                throw e;
            }else
            {
                return (IdentifierType)((STList)t[Name])[0];
            }
        }

        public static int GetCurrentTableCount()
        {
            return CurrentSymbolTable.Count;
        }
        private static Dictionary<string,object> GetParentTable(Dictionary<string,object> s)
        {
            return (Dictionary<string, object>)s["~ParentTable"];
        }

    }

    class STList : List<object>
    {
        public STList(IdentifierType Type,bool IfStatic,int Address,object Default,object ExtMsg1,object ExtMsg2, Dictionary<string, object> ChildTable)
        {
            this.Add(Type); //类型
            this.Add(IfStatic); //是否静态
            this.Add(Address); //储存位置
            this.Add(Default); //默认值
            this.Add(ExtMsg1); //附加信息1
            this.Add(ExtMsg2); //附加信息2
            this.Add(ChildTable); //子表引用

        }

    }
}
