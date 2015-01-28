using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using sendImg;

namespace MyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 10000;
                //string host = "127.0.0.1";
                string host = "121.42.136.178";
                //string host = "192.168.1.208";
                IPAddress ipa = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ipa, port);//把ip和端口转化为ipendpoint实例
                //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个socket
                //Console.WriteLine("connecting.....");
                //s.Connect(ipe);//连接到服务器
                //string sendStr = "";
                //sendStr = Console.ReadLine();
                //while(true)
                //{
                   
                    //byte[] bs = Encoding.ASCII.GetBytes(sendStr);
                    //Console.WriteLine("SendMessage");
                    //s.Send(bs, bs.Length, 0);//发送测试信息

                for (int i = 0; i < 1; i++)
                {
                    SendImg.SendImage("2.bmp", ipe);
                }

                Console.WriteLine("GOOD");
                Thread.Sleep(1000);
            }
            catch (ArgumentNullException ex)
            { Console.WriteLine(ex); }
            catch (SocketException ex)
            { Console.WriteLine(ex); }
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }
    }
}
