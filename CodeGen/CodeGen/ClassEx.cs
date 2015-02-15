using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen
{
    public static class ClassEx
    {
        public static readonly string newLine = Environment.NewLine;

        /// <summary>
        /// 在每行之前加上\t，即向右缩进
        /// </summary>
        /// <param name="builder"></param>
        public static StringBuilder InsertTabForEveryLine(this StringBuilder builder)
        {
            //当前环境中的换行是\n还是\r\n
            builder.Replace(newLine, newLine + "\t");
            builder.Insert(0, "\t");
            
            //因为是回车结尾：删除增加的\t
            builder = RemoveCRLFAtEnd(builder);

            return builder;
        }

        public static StringBuilder RemoveCRLFAtEnd(this StringBuilder builder)
        {
            return builder.Remove(builder.Length - newLine.Length, newLine.Length);
        }
    }
}
