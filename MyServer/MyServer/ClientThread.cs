using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyServer
{
    class ClientThread
    {
        private byte[] buf = new byte[1024];//服务器端设置缓冲区         
        private FileStream fileStream;
        private NetworkStream netStream;
        private TcpClient tcpClient;
        private string fileName;

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
            if (readCount == 0)
            {
                fileStream.Close();
                fileStream.Dispose();
                Console.WriteLine("File Received: " + fileName);

                int illType = Util.analyze(fileName);

                JObject jObj = new JObject(
                    new JProperty("userId", 123),
                    new JProperty("illType", new JArray(
                        1, 2, 4)));

                string jStr = jObj.ToString();
                byte[] bytes = Encoding.Default.GetBytes(jStr);

                netStream.Write(bytes, 0, bytes.Length);
                return;
            }

            fileStream.Write(buf, 0, readCount);

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
                if (netStream.DataAvailable)
                    netStream.BeginRead(buf, 0, buf.Length, ReadAsyncCallBack, null);

            }
            catch (Exception e)
            {
                Console.WriteLine("出现异常：{0}", e.ToString());
                Console.ReadLine();
            }
        }
    }
}
