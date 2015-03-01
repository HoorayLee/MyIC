using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamir.SharpSsh.jsch;

namespace dentists.helper
{
    class ShellHelp
    {
        System.IO.MemoryStream outputstream = new MemoryStream();
        Tamir.SharpSsh.SshStream inputstream = null;
        Channel channel = null;
        Session session = null;
        /// <summary> 
        /// 命令等待标识 
        /// </summary> 
        string waitMark = "]#";
        /// <summary> 
        /// 打开连接 
        /// </summary> 
        /// <param name="host"></param> 
        /// <param name="username"></param> 
        /// <param name="pwd"></param> 
        /// <returns></returns> 
        public bool OpenShell(string host, string username, string pwd)
        {
            try
            {
                ////Redirect standard I/O to the SSH channel 
                inputstream = new Tamir.SharpSsh.SshStream(host, username, pwd);
                ///我手动加进去的方法。。为了读取输出信息 
                return inputstream != null;
            }
            catch { throw; }
        }
        /// <summary> 
        /// 执行命令 
        /// </summary> 
        /// <param name="cmd"></param> 
        public bool Shell(string cmd)
        {
            if (inputstream == null) return false;
            inputstream.Write(cmd);
            inputstream.Flush();
            return true;
        }
        /// <summary> 
        /// 获取输出信息 
        /// </summary> 
        /// <returns></returns> 
        /// <summary> 
        /// 关闭连接 
        /// </summary> 
        public void Close()
        {
            if (inputstream != null) inputstream.Close();
        }
    }
}
