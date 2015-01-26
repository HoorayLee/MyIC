using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using System.Drawing;


class ClientThread
{
    private static object mylock = new object();
    Encoding encoding = Encoding.GetEncoding("UTF-8"); //解码器（可以用于汉字）         
    private static byte[] receiveBytes = new byte[1024];//服务器端设置缓冲区         

    static FileStream fs = new FileStream("C:\\Users\\leehorray\\Desktop\\image.jpg", FileMode.Create);
    public static NetworkStream ns;
    public static TcpClient client1;
    private static int nbThread = 0;
 
    //传递连接socket 
    public ClientThread(TcpClient ClientSocket)
    {
         client1 = ClientSocket;
    }

    static void ReadAsyncCallBack(IAsyncResult result)
    {
        int readCount;
        lock (mylock)
        {
            readCount = client1.GetStream().EndRead(result);
            if (readCount == 0)
            {
                fs.Close();
                ns.Dispose();
                return;
            }
        
      
             fs.Write(receiveBytes, 0, readCount);

             ns.BeginRead(receiveBytes, 0, 1024, ReadAsyncCallBack, null);
             Console.Write("Writing\n");
        }
        
    }
    //数据处理接口         
    public void ClientServer()
    {

        try
        {
            int i = 0;
            while (true)
            {
                ns = client1.GetStream();
                if (ns.DataAvailable )
                {
                    ns.BeginRead(receiveBytes, 0, 1024, ReadAsyncCallBack, null);
                    break;
                    Console.Write("Writing\n");
                }

                i++;
                if (i==10000000)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Write("出现异常：");
            Console.WriteLine(ex.ToString());
            Console.ReadLine();
        }
        
    }


    static void Main()
    {

        string HostName = Dns.GetHostName(); //得到主机名 
        IPHostEntry IpEntry = Dns.GetHostEntry(HostName); //得到主机IP 
        string strIPAddr = IpEntry.AddressList[0].ToString();
        IPAddress ip = IPAddress.Parse("127.0.0.1"); //把ip地址字符串转换为IPAddress              
        IPEndPoint ipep = new IPEndPoint(ip, 8080);//用指定的端口和ip  
        try
        {
            TcpListener listener = new TcpListener(ip, 8080);
            listener.Start();
       
       
       
        while (true)
        {
           
          
           
                //当有可用的客户端连接尝试时执行，并返回一个新的socket                  
                client1 = listener.AcceptTcpClient();
                //创建消息服务线程对象，并把连接socket赋于ClientThread                     
                ClientThread newclient = new ClientThread(client1);                     //把ClientThread 类的ClientService方法委托给线程 
                Thread newthread = new Thread(new ThreadStart(newclient.ClientServer));                     // 启动消息服务线程                    
                newthread.Start();
         }
        }
            catch
            {
                Console.Write("Connection has encountered a problem!");//连接中断或者连接失败                 
            }
        }
    }

