
using System;

using System.Collections.Generic;

using System.Text;

using System.Net;

using System.Net.Sockets;

using System.IO;

namespace sendImg
{
	class SendImg
	{
         public void SendImageToServer(String imgURl, String address)
            {
                if (!File.Exists(imgURl)) 
                return;
                FileStream fs = File.Open(imgURl, FileMode.Open);
                byte[] fileBytes = new byte[fs.Length];
            using (fs)
            {
                fs.Read(fileBytes, 0, fileBytes.Length);
                fs.Close();
            }
        
                TcpClient client = new TcpClient();
                client.Connect(address, 8080);
            using (client)
            {
                NetworkStream ns = client.GetStream();
                using (ns)
                {
                 ns.Write(fileBytes, 0, fileBytes.Length);
                }
            }
            }
	}
}
