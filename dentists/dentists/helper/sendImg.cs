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
using System.Net.Sockets;
namespace sendImg
{
    class SendImg
    {
        bool imagecomplet = false;
        Socket client;
        IPEndPoint hostentry;
        Boolean isfalse = false;
        BitmapImage image;
        WriteableBitmap images;
        public SendImg(WriteableBitmap a)
        {
            this.images = a;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            hostentry = new IPEndPoint(IPAddress.Parse("121.42.136.178"), 10000);
        }

        //public void sendImg()
        //{
            
        //}
        
        public void sendimagelength() 
        {
            SocketAsyncEventArgs socketeventarg = new SocketAsyncEventArgs();
            MemoryStream objMS = new MemoryStream();
            WriteableBitmap bitmap = images;
            Extensions.SaveJpeg(bitmap, objMS, bitmap.PixelWidth, bitmap.PixelHeight, 0, 50);
            byte[] send = BitConverter.GetBytes(objMS.GetBuffer().Length);
            socketeventarg.SetBuffer(send, 0, 2);
            socketeventarg.RemoteEndPoint = hostentry;
            socketeventarg.UserToken = client;
            socketeventarg.Completed += new EventHandler<SocketAsyncEventArgs>(sendlength);
            client.ConnectAsync(socketeventarg);
        }

        public void SendImageToServer(Socket a)
        {
            SocketAsyncEventArgs socketeventarg = new SocketAsyncEventArgs();
            MemoryStream objMS = new MemoryStream();
            WriteableBitmap bitmap = this.images;
            Extensions.SaveJpeg(bitmap, objMS, bitmap.PixelWidth, bitmap.PixelHeight, 0, 50);
            byte[] send = objMS.ToArray();
            socketeventarg.SetBuffer(send,0,send.Length);
            socketeventarg.RemoteEndPoint = hostentry;
            socketeventarg.UserToken = a;
            socketeventarg.Completed += new EventHandler<SocketAsyncEventArgs>(sendcomplet);
            imagecomplet = a.SendAsync(socketeventarg);
            SocketAsyncEventArgs socketreceiveevent = new SocketAsyncEventArgs();
            socketreceiveevent.RemoteEndPoint = a.RemoteEndPoint;
            socketreceiveevent.SetBuffer(new Byte[2048], 0, 2048);
            socketreceiveevent.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                    string response = null;
                    if (e.SocketError == SocketError.Success)
                    {
                        // Retrieve the data from the buffer
                        response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        response = response.Trim('\0');
                    }
                    else
                    {
                        response = e.SocketError.ToString();
                    }

            });
            client.ReceiveAsync(socketreceiveevent);
        }

        private void sendlength(object sender, SocketAsyncEventArgs e)
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
                        else
                        {
                            SendImageToServer(sock);
                        }
                    }
                }
                else
                {
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //{
                    //    MessageBox.Show("success");
                    //});
                    //if (e.UserToken != null)
                    //{
                    //    Socket sock = e.UserToken as Socket;
                    //    sock.Shutdown(SocketShutdown.Both);
                    //    sock.Close();
                    //}
                }
            }

        }

        private void sendcomplet(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success&&e.SocketError != SocketError.IsConnected)
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
                if (e.LastOperation == SocketAsyncOperation.Send)
                {
                    if (e.UserToken != null && !imagecomplet)
                    {
                        Socket sock = e.UserToken as Socket;
                        imagecomplet = sock.SendAsync(e);
                        if (!imagecomplet)
                        {
                            sendcomplet(e.UserToken, e);
                        }
                        else 
                        {
                            e.UserToken = null;
                        }
                    }
                }
                else 
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("success");
                    });
                //    if (e.UserToken != null)
                //    {
                //        Socket sock = e.UserToken as Socket;
                //        sock.Shutdown(SocketShutdown.Both);
                //        sock.Close();
                //    }   
                }
            }                                                                                                                                
        }
    }
}
