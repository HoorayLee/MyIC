using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json.Linq;

namespace sendImg
{
	class SendImg
	{
        public static void SendImage(String imgURL, IPEndPoint remoteEP)
        {
            if (!File.Exists(imgURL)) 
                return;

            //读取文件内容到缓冲区
            FileStream fs = File.Open(imgURL, FileMode.Open);
            byte[] fileBytes = new byte[fs.Length];
            fs.Read(fileBytes, 0, fileBytes.Length);
            fs.Close();
        
            //建立TCP连接
            TcpClient client = new TcpClient();
            client.Connect(remoteEP);

            //发送文件内容
            NetworkStream ns = client.GetStream();
            ns.Write(fileBytes, 0, fileBytes.Length);
            ns.Close();

            ////接收服务器返回信息
            //byte[] receivedBytes = new byte[1024];
            //ns = client.GetStream();
            //while (!ns.DataAvailable) ;
            //int readCount = ns.Read(receivedBytes, 0, receivedBytes.Length);
            //string jsonStr = Encoding.Default.GetString(receivedBytes, 0, readCount);
            ////JObject jsonObj = JObject.Parse(jsonStr);
            //Console.WriteLine(jsonStr);
            //ns.Close();
            //client.Close();

        }
	}
}
