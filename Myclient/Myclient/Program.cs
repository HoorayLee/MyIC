﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using sendImg;

namespace Myclient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 8080;
                string host = "127.0.0.1";
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

                    sendImg.SendImg Newpic = new sendImg.SendImg();
                    Newpic.SendImageToServer("C:\\Users\\leehorray\\Desktop\\期末啦logo.png", host);


                    //string recvStr = "";
                    //byte[] recvBytes = new byte[1024];
                    //int bytes;
                    //bytes = s.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接收返回信息
                    //recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
                    Console.WriteLine("GOOD");
                    Thread.Sleep(1000);
               // }
                //显示服务器返回信息
               
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
