using Coding4Fun.Toolkit.Controls;
using dentists.Data.IMdata;
using dentists.Page.im;
using dentists.Page.im.IMdata;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace IMClient_WinForm
{
    class MyIMClient
    {
        private static string serverIP = "121.42.136.178";
        private static int serverPort = 6789;
        //private static TcpClient client;
        public static Socket client;
        public static SocketAsyncEventArgs socketevent;
        public static SocketAsyncEventArgs receivevent;
        public static ImPage running;
        public static bool isconnect = false;
        public static bool Connected { get { return client.Connected; } }
        public static List<Msg> OfflineMsgs { get; set; }
        public static string Username { get; set; }
        private static IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        
        private static IPEndPoint serverEP;
        private static List<string> SplitIllegalJson(string json)
        {
            List<string> list = new List<string>();
            //从前向后找配对的{}
            int i, j = 0;
            int left = 0;
            for (i = 0; i < json.Length; i++)
            {
                if (json[i] == '{')
                {
                    if (j == 0)
                    {
                        left = i;
                    }
                    j++;
                }
                else if (json[i] == '}')
                {
                    j--;
                    if (j == 0)
                    {
                        list.Add(json.Substring(left, i - left + 1));
                    }
                }
            }
            return list;
        }
        public static void Login(string username)
        {
            Username = username;
            serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //首次登录
            JObject jObj = new JObject();
            jObj.Add("type", "0");
            jObj.Add("data", Username);
            byte[] buf = Encoding.UTF8.GetBytes(jObj.ToString());
            socketevent = new SocketAsyncEventArgs();
            socketevent.SetBuffer(buf, 0, buf.Length);
            socketevent.RemoteEndPoint = serverEP;
            socketevent.UserToken = client;
            socketevent.Completed += new EventHandler<SocketAsyncEventArgs>(sendfriend);
            client.ConnectAsync(socketevent);
        }

        public static bool SendMsg(Msg msg)
        {
            string msgJson = JsonConvert.SerializeObject(msg);
            JObject jObj = new JObject();
            jObj.Add("type", 1);
            jObj.Add("data", msgJson);
            byte[] buf = Encoding.UTF8.GetBytes(jObj.ToString());
            socketevent = new SocketAsyncEventArgs();
            socketevent.SetBuffer(buf, 0, buf.Length);
            socketevent.RemoteEndPoint = serverEP;
            socketevent.UserToken = client;
            socketevent.Completed += new EventHandler<SocketAsyncEventArgs>(sendfriend);
            client.SendAsync(socketevent);
            return true;
        }

        public static void Close()
        {
            client.Shutdown(SocketShutdown.Both);
        }

        private static void sendfriend(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                if (e.SocketError == SocketError.ConnectionAborted)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var toast = new ToastPrompt { Message = "connected timeout" };
                        toast.Visibility = System.Windows.Visibility.Visible;
                        toast.Show();
                    });
                }
                else if (e.SocketError == SocketError.HostNotFound)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var toast = new ToastPrompt { Message = "can't connect internet" };
                        toast.Visibility = System.Windows.Visibility.Visible;
                        toast.Show();
                    });
                }
                else if (e.SocketError == SocketError.ConnectionRefused)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var toast = new ToastPrompt { Message = "server is not online" };
                        toast.Visibility = System.Windows.Visibility.Visible;
                        toast.Show();
                    });
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var toast = new ToastPrompt { Message = "Time out" };
                        toast.Visibility = System.Windows.Visibility.Visible;
                        toast.Show();
                    });
                    Login(Username);
                }
            }
            else
            {
                if (e.LastOperation == SocketAsyncOperation.Connect)
                {
                    if (e.UserToken != null)
                    {
                        Socket sock = e.UserToken as Socket;
                        byte[] rec = new byte[2048];
                        receivevent = new SocketAsyncEventArgs();
                        receivevent.SetBuffer(rec, 0, rec.Length);
                        receivevent.RemoteEndPoint = serverEP;
                        receivevent.UserToken = client;
                        receivevent.Completed += new EventHandler<SocketAsyncEventArgs>(sendfriend);
                        sock.ReceiveAsync(receivevent);
                    }
                }
                else if (e.LastOperation == SocketAsyncOperation.Receive)
                {
                    if (e.UserToken != null)
                    {
                        Socket sock = e.UserToken as Socket;
                        string response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        if (response != "")
                        {
                            handlestring(response);
                            sock.ReceiveAsync(receivevent);
                        }
                        else 
                        {
                            Login(Username);
                        }
                    }
                    else
                    {
                        Login(Username);
                    }
                }
                //else if (e.LastOperation == SocketAsyncOperation.Send)
                //{
                //    if (e.UserToken != null)
                //    {
                //        Socket sock = e.UserToken as Socket;
                //        byte[] rec = new byte[2048];
                //        e.SetBuffer(rec, 0, rec.Length);
                //        sock.ReceiveAsync(e);
                //    }
                //}
            }
        }
        private static void handlestring(string a) 
        {
            List<string> jsonlist = SplitIllegalJson(a);
            foreach (string item in jsonlist)
            {
                JObject jObjRcv = JObject.Parse(item);
                int count = (int)jObjRcv["count"];
                if (count != 0)
                {
                    JArray jMsgs = JArray.Parse(jObjRcv["content"].ToString());
                    for (int i = 0; i < count; i++)
                    {
                        Msg msg = JsonConvert.DeserializeObject<Msg>(jMsgs[i].ToString());
                        if (iss.Contains(msg.From))
                        {
                            WordList list = iss[msg.From] as WordList;
                            list.wordlist.Add(msg);
                            if (running != null)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    if (running.id.Equals(msg.From))
                                    {
                                        running.uploadpage(1, msg.Content);
                                    }
                                });
                            }
                            else
                            {
                                list.wordnum++;
                                DoctorList doclist = iss["DoctorList"] as DoctorList;
                                doclist.totalunread++;
                            }

                            //iss[msg.From] = list;
                        }
                        else
                        {
                            List<Msg> t = new List<Msg>();
                            t.Add(msg);
                            WordList list = new WordList(t);
                            if (running != null)
                            {
                                if (running.name.Equals(msg.From))
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        running.uploadpage(1, msg.Content);
                                    });
                                }
                            }
                            else
                            {
                                list.wordnum++;
                                DoctorList doclist = iss["DoctorList"] as DoctorList;
                                doclist.totalunread++;
                            }
                            iss[msg.From] = list;
                        }
                    }
                }

            }
        }
    
    }
}
