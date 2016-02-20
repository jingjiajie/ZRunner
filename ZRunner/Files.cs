using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZRunner
{
    class Files
    {
        public byte[] ReadFile(string FilePath)
        {
            try
            {
                FileStream FS = new FileStream(FilePath,FileMode.Open,FileAccess.Read);
                int FLength = (int)FS.Length;
                byte[] buff = new byte[FLength];
                FS.Read(buff, 0, FLength);
                return buff;
            }catch (Exception e) { throw e; }
        }
    }
}
