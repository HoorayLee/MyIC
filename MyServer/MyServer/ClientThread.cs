using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyServer
{
    class ClientThread
    {
        private byte[] buf = new byte[1024];//服务器端设置缓冲区         
        private FileStream fileStream;
        private NetworkStream netStream;
        private TcpClient tcpClient;
        private string fileName;
        private long fileLen;
        private long readLen;

        private ClientThread() { }

        /// <summary>
        /// 文件接收线程辅助类构造函数
        /// </summary>
        /// <param name="tcpClient"></param>
        public ClientThread(TcpClient tcpClient, string fileName)
        {
            this.tcpClient = tcpClient;
            this.fileName = fileName;
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="result"></param>
        private void ReadAsyncCallBack(IAsyncResult result)
        {
            int readCount = netStream.EndRead(result);
            fileStream.Write(buf, 0, readCount);
            readLen += readCount;

            if (readLen == fileLen)
            {
                fileStream.Close();
                Console.WriteLine("File Received: " + fileName);

                MyServer.Util.Illnesses illType = Util.analyze(fileName);
                int nbTeeth = illType.nbTeeth;
                int[] ints = new int[nbTeeth];
                Marshal.Copy(illType.illnesses, ints, 0, nbTeeth);
                JObject jObj = new JObject(
                    new JProperty("userId", 123),
                    new JProperty("nbTeeth", nbTeeth),
                    new JProperty("illType", new JArray(ints)));

                string jStr = jObj.ToString();
                byte[] bytes = Encoding.Default.GetBytes(jStr);

                //将json返回给客户端
                if (netStream.CanWrite)
                {
                    netStream.Write(bytes, 0, bytes.Length);
                }
                return;
            }
            netStream.BeginRead(buf, 0, buf.Length, ReadAsyncCallBack, null);
        }

        /// <summary>
        /// 开始接收
        /// </summary>
        public void StartReceive()
        {
            try
            {
                netStream = tcpClient.GetStream();
                fileStream = new FileStream(fileName, FileMode.Create);
                while (!netStream.DataAvailable) ;
                byte[] bytes = new byte[sizeof(long)];
                netStream.Read(bytes, 0, bytes.Length);
                fileLen = BitConverter.ToInt64(bytes, 0);

                Console.WriteLine("{0}: {1} Bytes", fileName, fileLen);
                readLen = 0;
                //netStream.BeginRead(buf, 0, buf.Length, ReadAsyncCallBack, null);
                while (readLen < fileLen)
                {
                    int readCount = netStream.Read(buf, 0, buf.Length);
                    fileStream.Write(buf, 0, readCount);
                    readLen += readCount;
                }

                fileStream.Close();
                Console.WriteLine("File Received: " + fileName);

                MyServer.Util.Illnesses illType = Util.analyze(fileName);
                int nbTeeth = illType.nbTeeth;
                int[] ints = new int[nbTeeth];
                Marshal.Copy(illType.illnesses, ints, 0, nbTeeth);
                JObject jObj = new JObject(
                    new JProperty("userId", 123),
                    new JProperty("nbTeeth", nbTeeth),
                    new JProperty("illType", new JArray(ints)));

                string jStr = jObj.ToString();
                byte[] jBytes = Encoding.Default.GetBytes(jStr);

                //将json返回给客户端
                if (netStream.CanWrite)
                {
                    netStream.Write(jBytes, 0, jBytes.Length);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("出现异常：{0}", e.ToString());
                Console.ReadLine();
            }
        }
    }
}
