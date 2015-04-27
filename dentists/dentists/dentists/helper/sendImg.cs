
using System;

using System.Collections.Generic;

using System.Text;

using System.Net;

using System.Net.Sockets;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace sendImg
{
    class SendImg
    {
        Socket client;
        DnsEndPoint hostentry;
        Boolean isfalse = false;
        public void  SendImageToServer(BitmapImage a)
        {
            client = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            hostentry = new DnsEndPoint("192.168.1.208",10000);
            SocketAsyncEventArgs socketeventarg = new SocketAsyncEventArgs();
            MemoryStream objMS = new MemoryStream();
            WriteableBitmap bitmap = new WriteableBitmap(a);
            Extensions.SaveJpeg(bitmap, objMS, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
            byte[] send = objMS.GetBuffer();
            socketeventarg.SetBuffer(send,0,send.Length);
            socketeventarg.RemoteEndPoint = hostentry;
            socketeventarg.UserToken = client;
            socketeventarg.Completed += new EventHandler<SocketAsyncEventArgs>(sendcomplet);
            client.ConnectAsync(socketeventarg);
        }

        private void sendcomplet(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                if (isfalse == false)
                {
                    if (e.SocketError == SocketError.ConnectionAborted)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("connected timeout");
                        });
                        isfalse = true;
                    }
                    else if (e.SocketError == SocketError.HostNotFound)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("can't connect internet");
                        });
                    }
                    else if (e.SocketError == SocketError.ConnectionRefused)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("server is not online");
                        });
                        isfalse = true;
                    }
                    else
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("faild");
                        });
                        isfalse = true;
                    }
                }

            }
            else 
            {
                if (e.LastOperation == SocketAsyncOperation.Connect)
                {
                    if (e.UserToken != null)
                    {
                        Socket sock = e.UserToken as Socket;
                        bool completes = sock.SendAsync(e);
                        if (!completes)
                        {
                            sendcomplet(e.UserToken, e);
                        }
                    }
                }
                else 
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("success");
                    });
                    if (e.UserToken != null)
                    {
                        Socket sock = e.UserToken as Socket;
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                    }   
                }
            }
                                                                                                                                                
        }
    }
}
